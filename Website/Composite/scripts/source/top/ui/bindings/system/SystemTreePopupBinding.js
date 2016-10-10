SystemTreePopupBinding.prototype = new PopupBinding;
SystemTreePopupBinding.prototype.constructor = SystemTreePopupBinding;
SystemTreePopupBinding.superclass = PopupBinding.prototype;

SystemTreePopupBinding.CMD_CUT = "cut";
SystemTreePopupBinding.CMD_COPY = "copy";
SystemTreePopupBinding.CMD_PASTE = "paste";
SystemTreePopupBinding.CMD_REFRESH = "refresh";

/**
 * @type {boolean}
 */
SystemTreePopupBinding.isCutAllowed = false;
SystemTreePopupBinding.isRefreshAllowed = true;


/**
 * @class
 */
function SystemTreePopupBinding () {

	/**
	 * @type {SystemLogger}
	 */
	this.logger = SystemLogger.getLogger ( "SystemTreePopupBinding" );

	/**
	 * @type {string}
	 */
	this._currentProfileKey = null;

	/**
	 * @type {object}
	 */
	this._actionProfile = null;

	/**
	 * @type {SystemNode}
	 */
	this._node = null;

	/**
	 * @type {boolean}
	 */
	this._keepBundleState = false;

	/**
	 * @type {TreeNodeBinding}
	 */
	this.selectedTreeNodeBinding = null;
}

/**
 * @overloads {Binding#onBindingRegister}
 */
SystemTreePopupBinding.prototype.onBindingRegister = function () {

	SystemTreePopupBinding.superclass.onBindingRegister.call ( this );
	this.subscribe ( BroadcastMessages.SYSTEM_ACTIONPROFILE_PUBLISHED );
	this.addActionListener ( MenuItemBinding.ACTION_COMMAND, this );
}

/**
 * @overloads {Binding#onBindingAttach}
 */
SystemTreePopupBinding.prototype.onBindingAttach = function () {

	SystemTreePopupBinding.superclass.onBindingAttach.call ( this );
	this._indexMenuContent ();
}

/**
 * @implements {IBroadcastListener}
 * @param {string} broadcast
 * @param {object} arg
 */
SystemTreePopupBinding.prototype.handleBroadcast = function ( broadcast, arg ) {

	SystemTreePopupBinding.superclass.handleBroadcast.call ( this, broadcast, arg );

	switch ( broadcast ) {
		case BroadcastMessages.SYSTEM_ACTIONPROFILE_PUBLISHED :
			if (arg != null && arg.actionProfile != null) {
				//TODO: refactor with updating API
				this._node = arg.actionProfile.Node;
				this._actionProfile = arg.actionProfile;
			} else {
				this._currentProfileKey = null;
			}
			break;
	}
}

/**
 * @see {SystemToolbarBinding#_getProfileKey}
 * @return {string}
 */
SystemTreePopupBinding.prototype._getProfileKey = SystemToolBarBinding.prototype._getProfileKey

/**
 * @overloads {PopupBinding#show}
 */
SystemTreePopupBinding.prototype.show = function () {

	/*
	 * Build content
	 */
	var key = this._getProfileKey ();

	if ( key != this._currentProfileKey ) {
		this.disposeContent ();
		this.constructContent ();
		this._currentProfileKey = key;
	}

	this._setupClipboardItems ();
	this._setupRefreshItem ();

	/*
	 * Show.
	 */
	SystemTreePopupBinding.superclass.show.call ( this );
}

/**
 * Setup clipboard operation menuitems.
 */
SystemTreePopupBinding.prototype._setupClipboardItems = function () {

	var cut = this.getMenuItemForCommand ( SystemTreePopupBinding.CMD_CUT );
	var copy = this.getMenuItemForCommand ( SystemTreePopupBinding.CMD_COPY );
	var paste = this.getMenuItemForCommand ( SystemTreePopupBinding.CMD_PASTE );

	cut.setDisabled ( !SystemTreePopupBinding.isCutAllowed );
	copy.setDisabled ( !SystemTreePopupBinding.isCutAllowed );
	paste.setDisabled ( SystemTreeBinding.clipboard == null );
}

/**
 * Disable refresh on ghosted nodes.
 */
SystemTreePopupBinding.prototype._setupRefreshItem = function () {

	var refresh = this.getMenuItemForCommand ( SystemTreePopupBinding.CMD_REFRESH );
	refresh.setDisabled ( !SystemTreePopupBinding.isRefreshAllowed );
}

/**
 * @implements {IActionListener}
 * @overloads {PopupBinding#handleAction}
 * @param {Action} action
 */
SystemTreePopupBinding.prototype.handleAction = function ( action ) {

	SystemTreePopupBinding.superclass.handleAction.call ( this, action )

	switch ( action.type ) {

		/*
		 * Note to self: The first part is duplicated by SystemToolBarBinding!
		 */
		case MenuItemBinding.ACTION_COMMAND :
			var menuitemBinding = action.target;
			var systemAction = menuitemBinding.associatedSystemAction;

			if ( systemAction ) {
				SystemAction.invoke(systemAction, this._node);

				if(this._keepBundleState) {
					var bundleName = systemAction.getBundleName();
					if (bundleName != null) {
						LocalStorage.set(ToolBarComboButtonBinding.STORAGE_PREFFIX + bundleName, systemAction.getHandle());
					}
				}
			} else {
				var cmd = menuitemBinding.getProperty ( "cmd" );
				if ( cmd ) {
					this._handleCommand ( cmd );
				}
			}

			// Clean current profile key
			this._currentProfileKey = null;
			break;
	}
}

/**
 * Handle command.
 * @param {string} cmd
 */
SystemTreePopupBinding.prototype._handleCommand = function ( cmd ) {

	var broadcast = null;

	switch ( cmd ) {
		case SystemTreePopupBinding.CMD_CUT :
			broadcast = BroadcastMessages.SYSTEMTREEBINDING_CUT;
			break;
		case SystemTreePopupBinding.CMD_COPY :
			broadcast = BroadcastMessages.SYSTEMTREEBINDING_COPY;
			break;
		case SystemTreePopupBinding.CMD_PASTE :
			broadcast = BroadcastMessages.SYSTEMTREEBINDING_PASTE;
			break;
		case SystemTreePopupBinding.CMD_REFRESH :
			broadcast = BroadcastMessages.SYSTEMTREEBINDING_REFRESH;
			break;
	}

	if ( broadcast ) { // allows the popup to close
		setTimeout ( function () {
			EventBroadcaster.broadcast ( broadcast );
		}, 0 );
	}
}

/**
 * Dispose content; except clipboardoperations.
 */
SystemTreePopupBinding.prototype.disposeContent = function () {

	var members = new List (
		DOMUtil.getElementsByTagName ( this.bindingElement, "menugroup" )
	).reverse();
	while ( members.hasNext ()) {
		var binding = UserInterface.getBinding ( members.getNext ());
		if ( !binding.getProperty ( "rel" )) {
			binding.dispose ();
		}
	}
}

/**
 * Construct content.
 */
SystemTreePopupBinding.prototype.constructContent = function () {

	if ( this._actionProfile != null ) {

		var doc = this.bindingDocument;
		var groups = new List ();
		var self = this;

		var bundles = new Map();

		this._actionProfile.each ( function ( group, list ) {
			var groupBinding = MenuGroupBinding.newInstance ( doc );
			list.each ( function ( action ) {

				var menuItemBinding = self.getMenuItemBinding(action);

				var bundleName = action.getBundleName();
				if (bundleName) {
					if (bundles.has(bundleName)) {

						var bundleMenuItemBinding = bundles.get(bundleName);
						if (bundleMenuItemBinding.getChildBindingByType(MenuPopupBinding) == null) {
							bundleMenuItemBinding.setProperty("hasaction", true);
							var popup = MenuPopupBinding.newInstance(doc);
							var body = popup.add(MenuBodyBinding.newInstance(doc));
							var group = body.add(MenuGroupBinding.newInstance(doc));
							bundleMenuItemBinding.add(popup);
							if (this._keepBundleState || bundleMenuItemBinding.isDisabled) {
								group.add(self.getMenuItemBinding(bundleMenuItemBinding.associatedSystemAction));
							}
						}
						var bundleGroupMenuBinding = bundleMenuItemBinding.getDescendantBindingByType(MenuGroupBinding);
						if (bundleGroupMenuBinding) {
							bundleGroupMenuBinding.add(menuItemBinding);
						}
					} else {
						groupBinding.add(menuItemBinding);
						bundles.set(bundleName, menuItemBinding);
					}

				} else {

					groupBinding.add(menuItemBinding);
				}


			}, this);
			groups.add ( groupBinding );
		},this);

		/*
		 * Build in reverse order, so that clipboardoperations appear last.
		 * Remember that clipboard menuitems has been hardcoded into menu.
		 */
		groups.reverse ();
		while ( groups.hasNext ()) {
			this._bodyBinding.addFirst ( groups.getNext ());
		}
		this._bodyBinding.attachRecursive();

		bundles.each(function(bundleName, bundleMenuItemBinding) {
			if (bundleMenuItemBinding.isMenuContainer) {
				if (this._keepBundleState || bundleMenuItemBinding.isDisabled) {

					var bundleMenuItemsBinding = bundleMenuItemBinding.getDescendantBindingsByType(MenuItemBinding);
					var latestBundleMenuItem = null;

					if (this._keepBundleState) {
						var latestBundleHandle = LocalStorage.get(ToolBarComboButtonBinding.STORAGE_PREFFIX + bundleName);
						bundleMenuItemsBinding.each(function(menuItemBinding) {
							if (menuItemBinding.associatedSystemAction && menuItemBinding.associatedSystemAction.getHandle() === latestBundleHandle && !menuItemBinding.isDisabled) {
								latestBundleMenuItem = menuItemBinding;
								return false;
							}
							return true;
						});
					}

					if (latestBundleMenuItem == null) {
						bundleMenuItemsBinding.each(function(menuItemBinding) {
							if (!menuItemBinding.isDisabled) {
								latestBundleMenuItem = menuItemBinding;
								return false;
							}
							return true;
						});
					}

					if (latestBundleMenuItem == null) {
						latestBundleMenuItem = bundleMenuItemsBinding.getFirst();
					}

					this.setSystemAction(bundleMenuItemBinding, latestBundleMenuItem.associatedSystemAction);
					Binding.prototype.hide.call(latestBundleMenuItem);
				}
			}
		}, this);

	}
}

/**
 * @see {NavigatorToolBarBinding#getToolBarButtonBinding}
 * @param {SystemAction} action
 * @return {MenuItemBinding}
 */
SystemTreePopupBinding.prototype.getMenuItemBinding = function ( action ) {

	var binding 	= MenuItemBinding.newInstance ( this.bindingDocument );
	SystemTreePopupBinding.prototype.setSystemAction.call(this, binding, action);

	return binding;
}

SystemTreePopupBinding.prototype.setSystemAction = function (binding, action) {
	var label = action.getLabel();
	var tooltip = action.getToolTip();
	var image = action.getImage();
	var ximage = action.getDisabledImage();
	var isCheckbox = action.isCheckBox();

	if (label) {
		binding.setLabel(label);
	}
	if (tooltip) {
		binding.setToolTip(tooltip);
	}
	if (image) {
		binding.imageProfile = new ImageProfile({
			image: image,
			imageDisabled: ximage
		});
	}
	if (isCheckbox) {
		binding.setType(MenuItemBinding.TYPE_CHECKBOX);
		if (action.isChecked()) {
			binding.check(true);
		} else {
			binding.uncheck(true);
		}
	}
	if (action.isDisabled()) {
		binding.disable();
	} else if(binding.isDisabled) {
		binding.enable();
	}

	/*
	 * Stamp the action as a property on the menuitem
	 * so that we can retrieve it when the item is clicked.
	 */
	binding.associatedSystemAction = action;
}

/**
 * @overwrites {PopupBinding#snapToMouse}
 * @param {MouseEvent} e
 */
SystemTreePopupBinding.prototype.snapToMouse = function ( e ) {

	var node = e.target ? e.target : e.srcElement;
	var name = DOMUtil.getLocalName ( node );
	var binding = null;

	if ( name != "tree" ) {
		switch ( name ) {
			case "treenode" :
				// visually empty space - this should not open the popup!
				break;
			default :
				node = DOMUtil.getAncestorByLocalName ( "treenode", node );
				if ( node != null ) {
					binding = UserInterface.getBinding ( node );
					if ( binding.isDisabled ) { // no contextmenu for disabled treenodes
						binding = null;
					}
				}
				break;
		}
		if ( binding != null && binding.node != null && binding.node.getActionProfile () != null ) {

			/*
			 * This timeout will allow a right-click to focus the treenode,
			 * triggering a NEW action profile publish, before we show the menu.
			 * OOPS: Internet Explorer looses track of the e argument... what to do?
			 *
			var self = this;
			setTimeout ( function () {
				SystemTreePopupBinding.superclass.snapToMouse.call ( self, e );
			}, 0 );
			*/

			SystemTreePopupBinding.superclass.snapToMouse.call ( this, e );
		}
	}
}

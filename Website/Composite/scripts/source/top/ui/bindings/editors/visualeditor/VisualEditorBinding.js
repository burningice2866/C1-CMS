VisualEditorBinding.prototype = new EditorBinding;
VisualEditorBinding.prototype.constructor = VisualEditorBinding;
VisualEditorBinding.superclass = EditorBinding.prototype;

VisualEditorBinding.FUNCTION_CLASSNAME = "compositeFunctionWysiwygRepresentation";
VisualEditorBinding.FIELD_CLASSNAME = "compositeFieldReferenceWysiwygRepresentation";
VisualEditorBinding.HTML_CLASSNAME = "compositeHtmlWysiwygRepresentation";

VisualEditorBinding.ACTION_INITIALIZED = "visualeditor initialized";
VisualEditorBinding.DEFAULT_CONTENT = "<p><br/></p>";
VisualEditorBinding.URL_DIALOG_CONTENTERROR = "${root}/content/dialogs/wysiwygeditor/errors/contenterror.aspx";
VisualEditorBinding.XHTML = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\n\t<head>${head}</head>\n\t<body>\n${body}\n\t</body>\n</html>";

/*
 * It ain't pretty, but we had to put this somewhere.
 * @param {string} classname
 * @return {string}
 */
VisualEditorBinding.getTinyLessClassName = function (classname) {

	var i = 0, singlename, result = [], split = classname.split(" ");
	while ((singlename = split[i++]) != null) {
		if (singlename.length >= 3 && singlename.substring(0, 3) == "mce") {
			continue;
		} else if (singlename.length >= 14 && singlename.substring(0, 14) == "compositemedia") {
			continue;
		}
		result.push(singlename);
	}
	return result.join(" ");
}

/**
 * Convert tinymarkup to structured markup.
 * @param {string} content
 * @return {string}
 */
VisualEditorBinding.getStructuredContent = function ( content ) {
	
	var result = null;
	WebServiceProxy.isFaultHandler = false;
	var soap = XhtmlTransformationsService.TinyContentToStructuredContent ( content );
	if ( soap instanceof SOAPFault ) {
		// DO SOMETHING!?
	} else {
		result = soap.XhtmlFragment;
		if ( !result ) {
			result = "";
		}
	}
	WebServiceProxy.isFaultHandler = true;
	return result;
}

/**
 * Convert structured markup to tinymarkup.
 * @param {string} content Structured markup
 * @param {VisualEditorBinding} binding
 * @return {string}
 */
VisualEditorBinding.getTinyContent = function ( content, binding ) {
	
	var result = null;
	
	/*
	 * Some content seems to be needed for the webservice to return valid fragment.
	 */
	if ( content == null || content == "" ) {
		content = VisualEditorBinding.DEFAULT_CONTENT;
	}
	
	/*
	 * If webservice fails to convert structured markup,
	 * a dialog will be presented and null will be returned.
	 */
	WebServiceProxy.isFaultHandler = false;
	var soap = XhtmlTransformationsService.StructuredContentToTinyContent ( content );
	if ( soap instanceof SOAPFault ) {
		var dialogArgument = soap;
		var dialogHandler = {
			handleDialogResponse : function () {
				/*
				 * Otherwise the save button could be disabled 
				 * indefinitely during save scenario
				 */
				binding.dispatchAction ( Binding.ACTION_VALID );
			}
		};
		Dialog.invokeModal (
			VisualEditorBinding.URL_DIALOG_CONTENTERROR,
			dialogHandler, 
			dialogArgument 
		);
	} else {
		result = soap.XhtmlFragment;
		if ( result == null ) { // always return a string!
			result = new String ( "" );
		}
	}
	WebServiceProxy.isFaultHandler = true;
	return result;
};

/**
 * Extract stuff from HTML. This entire operation is carried out in elaborate DOM,  
 * not string parsing, in order to maintain proper namespaces on elements. Otherwise 
 * we would be in trouble eg. if user moved the Function namespace to other tags. 
 * @param {string} html
 * @param {int} index
 * @return {string}
 */
VisualEditorBinding.extractByIndex = function ( html, index ) {
	
	/*
	 * TODO: We crate two DOM documents, one for HEAD and BODY each, where 
	 * only one is needed. This is expensive and should be rationalized...
	 */
	var result = null;
	var doc = XMLParser.parse ( html );
	
	if ( doc != null ) {
		
		/* For some lame reason, IE cannot find elements in a DOM document based 
		 * on their node name. This must be something new and absurd. Instead, we 
		 * locate the element by ordinal index and forget about it. But this is 
		 * seriously twisted, so consider looking into it...
		 */
		var children = new List ( doc.documentElement.childNodes );
		var elements = new List ();
		children.each ( function ( child ) {
			if ( child.nodeType == Node.ELEMENT_NODE ) {
				elements.add ( child );
			}
		})
		
		var target = elements.get ( index );
		if ( target == null ) {
			if ( Application.isDeveloperMode ) {
				alert ( "VisualEditorBinding: Bad HTML!" + "\n\n" + html );
			}
		} else if ( target.hasChildNodes ()) {
			
			/*
			 * Move target nodes to temporary document fragment.
			 */
			var frag = doc.createDocumentFragment ();
			while ( target.hasChildNodes ()) {
				frag.appendChild ( target.firstChild );
			}
			
			/*
			 * Empty the document and move target nodes back. We now have the  
			 * target seaction elements isolated with proper namespaces declared.
			 */
			doc.removeChild ( doc.documentElement );
			doc.appendChild ( DOMUtil.createElementNS ( Constants.NS_XHTML, "ROOT", doc ));
			doc.documentElement.appendChild ( frag );
			
			/*
			 * Finally we serialize the result. It may contain ugly   
			 * whitespace, but this will be dealt with elsewhere.
			 */
			result = DOMSerializer.serialize ( doc.documentElement );
			result = result.substring ( result.indexOf ( ">" ) + 1, result.length );
			result = result.substring ( 0, result.lastIndexOf (  "<" ));
		}
	}
	
	/*
	 * We don't want to return a null here.
	 */
	if ( result == null ) {
		result = new String( "" );
	}
	return result;
}



/**
 * Is image?
 * @param {DOMElement} element
 * @return {boolean}
 */
VisualEditorBinding.isImage = function (element) {

	result = element && element.nodeName == "IMG";
	return result;
}

/**
* Is image and not rendering?
* @param {DOMElement} element
* @return {boolean}
*/
VisualEditorBinding.isImageElement = function (element) {
	return VisualEditorBinding.isImage(element) && !VisualEditorBinding.isReservedElement(element);
}

/**
 * Is internal image element?
 * @param {DOMElement} element
 * @return {boolean}
 */
VisualEditorBinding.isReservedElement = function (element) {
	if (VisualEditorBinding.isFunctionElement(element))
		return true;
	if (VisualEditorBinding.isFieldElement(element))
		return true;
	if (VisualEditorBinding.isHtmlElement(element))
		return true;
	return false; 
}


/**
 * Is function element?
 * @param {DOMElement} element
 * @return {boolean}
 */
VisualEditorBinding.isFunctionElement = function (element) {

	return VisualEditorBinding.isImage(element) &&
		CSSUtil.hasClassName (
			element,
			VisualEditorBinding.FUNCTION_CLASSNAME
		);
}


/**
 * Is field element?
 * @param {DOMElement} element
 * @return {boolean}
 */
VisualEditorBinding.isFieldElement = function (element) {

	return VisualEditorBinding.isImage(element) &&
		CSSUtil.hasClassName (
			element,
			VisualEditorBinding.FIELD_CLASSNAME
		);
}

/**
 * Is html element?
 * @param {DOMElement} element
 * @return {boolean}
 */
VisualEditorBinding.isHtmlElement = function (element) {

	return VisualEditorBinding.isImage(element) &&
		CSSUtil.hasClassName (
			element,
			VisualEditorBinding.HTML_CLASSNAME
		);
}


/**
 * @class
 */
function VisualEditorBinding () {
	
	/**
	 * @type {SystemLogger}
	 */
	this.logger = SystemLogger.getLogger ( "VisualEditorBinding" );
	
	/**
	 * @type {string}
	 */
	this.action_initialized = VisualEditorBinding.ACTION_INITIALIZED;
	
	/**
	 * @type {string}
	 */
	this.url_default = "${root}/content/misc/editors/visualeditor/visualeditor.aspx";
	
	/**
	 * The TinyMCE engine.
	 * @type {TinyMCE_Engine} 
	 */
	this._tinyEngine = null;
	
	/**
	 * The TinyMCE instance.
	 * @type {tinymce.Editor}
	 */
	this._tinyInstance = null;
	
	/**
	 * The TinyMCE theme.
	 * @type {TinyMCE_CompositeTheme}
	 */
	this._tinyTheme = null;
	
	/**
	 * @type {VisualEditorFieldGroupConfiguration}
	 */
	this.embedableFieldConfiguration = null;

	/**
	 * OLD STUFF HERE!
	 * 
	 * 
	 * @type {VisualEditorFormattingConfiguration}
	 *
	this.formattingConfiguration = null;
	*/
	
	/**
	 * TinyMCE internal stylesheet. Externalized  
	 * so that an TinyMCE upgrade won't overwrite.
	 *
	this.defaultStylesheet = VisualEditorBinding.DEFAULT_STYLESHEET;
	
	/**
	 * Editor configuration stylesheet URL.
	 * @type {string}
	 *
	this.configurationStylesheet = null;
	
	/**
	 * Editor presentation stylesheet URL.
	 * @type {string}
	 *
	this.presentationStylesheet = null;
	*/
	
	/**
	 * Stores the HEAD section markup.
	 * @type {string}
	 */
	this._head = null;
	
	/*
	 * Returnable.
	 */
	return this;
}

/**
 * @overloads {EditorBinding#onBindingRegister}
 */
VisualEditorBinding.prototype.onBindingRegister = function () {
	
	/* 
	 * Force an early indexation of VisualEditorBinding strings  
	 * to supress occasional glitches in string fetching.
	 */
	VisualEditorBinding.superclass.onBindingRegister.call ( this );
	
	// load strings
	StringBundle.getString ( "Composite.Web.VisualEditor", "Preload.Key" );

	
}

/**
 * @overloads {WindowBinding#onBindingAttach}
 */
VisualEditorBinding.prototype.onBindingAttach = function () {
	
	// fields config
	var fieldsconfig = this.getProperty ( "embedablefieldstypenames" );
	if ( fieldsconfig != null ) {
		this.embedableFieldConfiguration = VisualEditorFieldGroupConfiguration.getConfiguration ( fieldsconfig );
	}
	
	// formatting config
	var config = this.getProperty ( "formattingconfiguration" );
	if ( config != null ) {
		this._url += "?config=" + config;
	}
	
	VisualEditorBinding.superclass.onBindingAttach.call ( this );
	
	this.subscribe ( BroadcastMessages.TINYMCE_INITIALIZED );
	
	// this._parseDOMProperties ();
};

/**
 * Identifies binding.
 */
VisualEditorBinding.prototype.toString = function () {

	return "[VisualEditorBinding]";
};

/**
 * Parse DOM properties.
 *
VisualEditorBinding.prototype._parseDOMProperties = function () {

	var presentation = this.getProperty ( "presentationstylesheet" );
	if ( presentation != null ) {
		this.presentationStylesheet = presentation;
	}
	
	var configuration = this.getProperty ( "configurationstylesheet" );
	if ( configuration != null ) {
		this.configurationStylesheet = configuration;
	}
	
	var classconfig = this.getProperty ( "elementclassconfiguration" );
	if ( classconfig != null ) {
		this.elementClassConfiguration = VisualEditorElementClassConfiguration.getConfiguration ( classconfig );
	}
	
	var formatconfig = this.getProperty ( "formattingconfiguration" );
	if ( formatconfig != null ) {
		this.formattingConfiguration = VisualEditorFormattingConfiguration.getConfiguration ( formatconfig );
	}

	var fieldsconfig = this.getProperty ( "embedablefieldstypenames" );
	if ( fieldsconfig != null ) {
		this.embedableFieldConfiguration = VisualEditorFieldGroupConfiguration.getConfiguration ( fieldsconfig );
	}
};
*/

/**
 * @implements {IBroadcastListener}
 * @param {string} broadcast
 * @param {object} arg
 */
VisualEditorBinding.prototype.handleBroadcast = function ( broadcast, arg ) {
	
	VisualEditorBinding.superclass.handleBroadcast.call ( this, broadcast, arg );
	
	var windowBinding = this.getContentWindow ().bindingMap.tinywindow;
	var contentWindow = windowBinding.getContentWindow ();
	
	switch ( broadcast ) {

		/*
		 * TinyMCE initialized.
		 */
		case BroadcastMessages.TINYMCE_INITIALIZED :
			
			if ( arg.broadcastWindow == contentWindow ) {

				this._tinyEngine = arg.tinyEngine;
				this._tinyInstance = arg.tinyInstance;
				this._tinyTheme = arg.tinyTheme;

				this._tinyTheme.initC1(
					this,
					this._tinyEngine,
					this._tinyInstance
				);

				/*
				* Some kind of devilry going on with the server...
				*/
				if (this._startContent == " ") {
					this._startContent = VisualEditorBinding.DEFAULT_CONTENT;
				}

				/*
				* Normalize start content and extract HEAD and BODY section before we 
				* feed it to TinyMCE. Normalization is required while old solutions 
				* are upgraded to the new setup (with HEAD and BODY sections). 
				*/
				this._startContent = this.normalizeToDocument(this._startContent);
				this.extractHead(this._startContent);
				this._startContent = this.extractBody(this._startContent);

				/*
				* Inject BODY markup into TinyMCE. From now on, injection  
				* is handled by the VisualEditorPageBinding.
				*/
				arg.tinyInstance.setContent(VisualEditorBinding.getTinyContent(this._startContent), { format: 'raw' });



				this.initializeEditorComponents ( windowBinding );
				this._initialize ();
				
				this.unsubscribe ( BroadcastMessages.TINYMCE_INITIALIZED );
			}
			break;
	}
};

/**
 * Initialize components collected during startup. After startup, 
 * this method is invoked directly when bindings register themselves 
 * through method EditorBinding.registerComponent.
 * @param {IEditorComponent} binding
 */
VisualEditorBinding.prototype.initializeEditorComponent = function ( binding ) {

	binding.initializeComponent (
		this,
		this._tinyEngine,
		this._tinyInstance,
		this._tinyTheme
	);
};

/**
 * @overloads {EditorBinding#finalize}
 */
VisualEditorBinding.prototype._finalize = function () {
	
	VisualEditorBinding.superclass._finalize.call ( this );
	this._maybeShowEditor ();
};

/**
 * Invoked when contained page initializes.
 * @overloads {EditorBinding#_onPageInitialze}
 * @param {PageBinding} binding
 */
VisualEditorBinding.prototype._onPageInitialize = function ( binding ) {
	
	VisualEditorBinding.superclass._onPageInitialize.call ( this, binding );
	this._maybeShowEditor ();
};

/**
 * Stuff is not always loaded in a tight sequence arund here, so  
 * we make sure not to show the editor until we are ready. 
 */
VisualEditorBinding.prototype._maybeShowEditor = function () {
	
	if ( this._isFinalized && this._pageBinding != null ) {
		this._checksum = this.getCheckSum ();
		this._pageBinding.showEditor ( true );
	}
};

/**
 * Backup HEAD section from HTML document markup. 
 * This must be done whenever TinyMCE gets served. 
 * @param {string} html
 */
VisualEditorBinding.prototype.extractHead = function ( html ) {
	
	this._head = VisualEditorBinding.extractByIndex ( html, 0 );
}

/**
 * Extract BODY section and return it. TinyMCE 
 * should alwasy be fed BODY content only.
 * @param {string} html
 * @return {string}
 */
VisualEditorBinding.prototype.extractBody = function ( html ) {
	
	return VisualEditorBinding.extractByIndex ( html, 1 );
}

/**
 * Restore HEAD section and convert HTML fragment to normalized HTML document.   
 * This must be done whenever content is extracted from TinyMCE.
 * @param {string} body
 * @return {string}
 */
VisualEditorBinding.prototype.normalizeToDocument = function ( markup ) {
	
	var result = markup;
	if ( !this._isNormalizedDocument ( markup )) {
		result = VisualEditorBinding.XHTML
			.replace ( "${head}", this._getHeadSection ())
			.replace ( "${body}", markup );
	}
	return result;
}

/**
 * Is markup a valid HTML document; or simply a fragment?
 * @param {string} markup
 * @return {boolean}
 */
VisualEditorBinding.prototype._isNormalizedDocument = function ( markup ) {
	
	var result = false;
	var doc = XMLParser.parse ( markup, true );
	if ( doc != null ) {
		if ( doc.documentElement.nodeName == "html" ) {
			result = true;
		}
	}
	//When markup start with <!-- --> then parser return html document in chrome
	//TODO: Investigate it to make function more niced
	if (Client.isWebKit) {
		if (markup.indexOf("<html") !== 0) {
			result = false;
		}
	}
	return result;
}

/**
 * Get cached HEAD section. Method isolated so that subclasses may overwrite.
 * @return {string}
 */
VisualEditorBinding.prototype._getHeadSection = function () {
	
	return this._head != null ? this._head : new String ( "" );
}

/**
 * Handle command.
 * @overwrites {EditorBinding#handleCommand}
 * @param {string} cmd
 * @param {boolean} gui
 * @param {string} val
 * @return {boolean} ... This is always true; maybe refactor something?
 */
VisualEditorBinding.prototype.handleCommand = function ( cmd, gui, val ) {
	
	/*
	 * The superclass handles special commmands "copy" and "paste" 
	 * thay may invoke a warning dialog in unprivileged Mozillas.
	 */
	var isCommandHandled = VisualEditorBinding.superclass.handleCommand.call ( this, cmd, gui, val );
	
	/*
	 * Otherwise, the command gets realyed to the TinyMCE instance.
	 */
	if ( !isCommandHandled ) {
		try {
			this._tinyInstance.execCommand ( cmd, gui, val );
			this.checkForDirty ();
		} catch ( e ) {
			SystemDebug.stack ( arguments );
		}
		isCommandHandled = true;
	}
	
	return isCommandHandled;
};

/**
 * Configure contextmenu before showing it.
 * @overloads {EditorBinding#handleContextMenu}
 * @param {MouseEvent} e
 */
VisualEditorBinding.prototype.handleContextMenu = function ( e ) {

	var element = DOMEvents.getTarget ( e );
	this._popupBinding.configure ( this._tinyInstance, this._tinyEngine, element );
	VisualEditorBinding.superclass.handleContextMenu.call ( this, e );
}



// ABSTRACT METHODS IMPLEMENTED .............................................

/**
 * Get the editable window.
 * @return {DOMDocumentView}
 */
VisualEditorBinding.prototype.getEditorWindow = function () {
	
	return DOMUtil.getParentWindow ( this.getEditorDocument ());
};

/**
 * Get the editable document.
 * @return {DOMDocument}
 */
VisualEditorBinding.prototype.getEditorDocument = function () {
	
	return this._tinyInstance.getDoc ();
};

/**
 * Get the contextmenu associated.
 * @return {VisualEditorPopupBinding}
 */
VisualEditorBinding.prototype.getEditorPopupBinding = function () {
	
	return app.bindingMap.visualeditorpopup;
};

/**
 * Create selection bookmark.
 */
VisualEditorBinding.prototype.createBookmark = function () {
	
	this._bookmark = this._tinyInstance.selection.getBookmark ( true );
};

/**
 * Restore selection from bookmark. This will delete the bookmark!
 */
VisualEditorBinding.prototype.restoreBookmark = function () {
	
	if ( this.hasBookmark ()) {
		this._tinyInstance.selection.moveToBookmark ( this._bookmark );
		this.deleteBookmark ();
	}
};

/**
 * Has bookmark?
 * @return {boolean}
 */
VisualEditorBinding.prototype.hasBookmark = function () {
	
	return this._bookmark != null;
};

/**
 * Delete bookmark.
 */
VisualEditorBinding.prototype.deleteBookmark = function () {
	
	this._bookmark = null;
};

/**
 * Reset undo-redo history.
 */
VisualEditorBinding.prototype.resetUndoRedo = function () {

    this._tinyInstance.undoManager.clear();
    this._tinyInstance.undoManager.add();
	if ( this._pageBinding != null ) {
		this._pageBinding.updateUndoBroadcasters ();
	}
};

/**
 * Used to determine when a dirty flag should be raised.
 * @return {string}
 *
VisualEditorBinding.prototype.getCheckSum = function () {
	
	var result = null;
	if ( Binding.exists ( this._pageBinding )) {
		result = this._pageBinding.getCheckSum ( this._checksum );
	}
	return result;
}
*/


// IMPLEMENT AS DATABINDING .............................................

/**
 * Validate.
 * @implements {IData}
 * @return {boolean}
 */
VisualEditorBinding.prototype.validate = function () {
	
	return this._pageBinding.validate ();
};

/**
 * Get value. This is intended for serversice processing.
 * @implements {IData}
 * @return {string}
 */
VisualEditorBinding.prototype.getValue = function () {
	
	/*
	 * The content is probably valid at this point because the validate 
	 * method has been invoked. We can save some time here by not duplicating 
	 * validation, although theoretically we should.
	 */
	return this._pageBinding.getContent ();
};

/**
 * Set value. This resets the undo stack.
 * @param {string} value
 */
VisualEditorBinding.prototype.setValue = function ( value ) {
	
	if ( this._isFinalized ) {
		if ( Binding.exists ( this._pageBinding )) {
			this._pageBinding.setContent ( value );
			// resetUndoRedo invoked by page!
			// _checksum reset by page! (how ugly!)
		}
	} else if ( this._startContent == null ){
		this._startContent = value;
	}
};

/**
 * Get result. This is intended for clientside processing.
 * @implements {IData}
 * @return {object}
 */
VisualEditorBinding.prototype.getResult = function () {};

/**
 * Clean must be realyed to sourcecodeeditor.
 * @overloads {EditorBinding#clean}
 */
VisualEditorBinding.prototype.clean = function () {
	
	VisualEditorBinding.superclass.clean.call ( this );
	
	if ( this._pageBinding != null ) {
		this._pageBinding.clean ();
	}
}

/**
 * Set result. This is intended for clientside processing.
 * @param {string} result
 */
VisualEditorBinding.prototype.setResult = function ( result ) {};
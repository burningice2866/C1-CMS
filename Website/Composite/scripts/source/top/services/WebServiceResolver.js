WebServiceResolver.prototype = new XPathResolver;
WebServiceResolver.prototype.constructor = WebServiceResolver;
WebServiceResolver.superclass = XPathResolver.prototype;

/**
 * @class
 * @param {string} url
 */
function WebServiceResolver ( url ) {

	/**
	 * @type {Logge
	 */
	this.logger = SystemLogger.getLogger ( "WebServiceResolver" );
	
	/**
	 * @type {DOMElement}
	 */
	this._root = this._getDocumentElement ( url );
	
	/**
	 * @type {Schema}
	 */
	this._schema = null;
	
	
	if ( this._root ) {
	
		this.setNamespacePrefixResolver ({
			"wsdl"	: Constants.NS_WSDL,
			"soap"	: Constants.NS_SOAP,
			"s" 	: Constants.NS_SCHEMA
		});
		
		this._schema = new Schema ( 
			this.resolve ( "wsdl:types/s:schema", this._root )
		);
	}
	
	/**
	 * We store this in order to hack the "getPortAddress" method below...
	 * @param {string} url
	 */
	this._WSDLURL = url;
}

/**
 * @param {string} url
 * return {DOMElement}
 */
WebServiceResolver.prototype._getDocumentElement = function ( url ) {

	var result = null;
	var request = DOMUtil.getXMLHTTPRequest ();
	request.open ( "get", url, false );
	request.send ( null );
	if ( request.responseXML ) {
		result = request.responseXML.documentElement;
	} else {
		alert ( request.responseText );
		throw new Error ( "WebServiceResolver: Could not read WSDL: " + url );
	}
	return result;
}

/**
 * Get webservice address.
 * @return {string}
 */
WebServiceResolver.prototype.getPortAddress = function () {
	
	/*
	 * Because of issues with certain cheap proxy servers, we don't extract  
	 * the webservice address from the WSDL. Instead we retrieve from the 
	 * from the given WSDL-address by hacking it the hardcode way... 
	 * 
	var soapAddress = this.resolve ( "wsdl:service/wsdl:port/soap:address", this._root );
	return soapAddress.getAttribute ( "location" );
	*/
	
	/*
	 * Hope MS doesn't change this convention...
	 */
	return this._WSDLURL.split ( "?WSDL" )[ 0 ];
}

/**
 * Get webservice namespace.
 * @return {string}
 */
WebServiceResolver.prototype.getTargetNamespace = function () {

	return this._root.getAttribute ( "targetNamespace" );
}

/**
 * Get webservice operations.
 * @return {List}
 */
WebServiceResolver.prototype.getOperations = function () {

	var result		= new List ();
	var elements 	= this.resolveAll ( "wsdl:portType/wsdl:operation", this._root ); // "wsdl:portType[@name='WebServicesSoap']/wsdl:operation"
	
	if ( elements.hasEntries ()) { 
		while ( elements.hasNext ()) {
		
			var element	= elements.getNext ();
			var name = element.getAttribute ( "name" );
			
			result.add (
				new WebServiceOperation ( 
					name,
					this.getPortAddress (),
					new SOAPEncoder ( this, name ),
					new SOAPDecoder ( this, name )
				)
			);
		}
	} else {
		
		/*
		 * This specific portype name is autogenerated by the NET webservice engine.
		 */
		throw new Error ( "WebServiceResolver: No portType found." );
	}
	return result;
}

/**
 * @return {Schema}
 */
WebServiceResolver.prototype.getSchema = function () {
	
	return this._schema;
}
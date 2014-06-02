<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [ <!ENTITY nbsp "&#x00A0;"> ]>
<xsl:stylesheet 
  version="1.0" 
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
  xmlns:msxml="urn:schemas-microsoft-com:xslt" 
  xmlns:umbraco.library="urn:umbraco.library" xmlns:Exslt.ExsltCommon="urn:Exslt.ExsltCommon" xmlns:Exslt.ExsltDatesAndTimes="urn:Exslt.ExsltDatesAndTimes" xmlns:Exslt.ExsltMath="urn:Exslt.ExsltMath" xmlns:Exslt.ExsltRegularExpressions="urn:Exslt.ExsltRegularExpressions" xmlns:Exslt.ExsltStrings="urn:Exslt.ExsltStrings" xmlns:Exslt.ExsltSets="urn:Exslt.ExsltSets" xmlns:umbraco.contour="urn:umbraco.contour" xmlns:tagsLib="urn:tagsLib" xmlns:BlogLibrary="urn:BlogLibrary" 
  exclude-result-prefixes="msxml umbraco.library Exslt.ExsltCommon Exslt.ExsltDatesAndTimes Exslt.ExsltMath Exslt.ExsltRegularExpressions Exslt.ExsltStrings Exslt.ExsltSets umbraco.contour tagsLib BlogLibrary ">

<xsl:output method="xml" omit-xml-declaration="yes"/>

<xsl:param name="currentPage"/>

<xsl:template match="/">

<!-- The fun starts here -->

<div class="newsList">
<xsl:for-each select="$currentPage/* [@isDoc and string(umbracoNaviHide) != '1']">
  <xsl:sort select="@updateDate" order="ascending" />
   <h3 class="headline"> <a href="{umbraco.library:NiceUrl(@id)}">
      <xsl:value-of select="@nodeName"/>
    </a>
  </h3>
  <small class="meta">Posted: <xsl:value-of select="umbraco.library:LongDate(@updateDate, true(), ' - ')"/></small><br/>
  <p class="introduction">
    <xsl:value-of select="umbraco.library:ReplaceLineBreaks(introduction)" disable-output-escaping="yes"/>
  </p>
  
</xsl:for-each>
  </div>

</xsl:template>

</xsl:stylesheet>
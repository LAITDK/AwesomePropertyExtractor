<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [
  <!ENTITY nbsp "&#x00A0;">
]>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxml="urn:schemas-microsoft-com:xslt"
  xmlns:umbraco.library="urn:umbraco.library"
  exclude-result-prefixes="msxml umbraco.library">


  <xsl:output method="html" omit-xml-declaration="yes" />

  <xsl:param name="currentPage"/>
  
  <!-- Input the related links property alias here -->
  <xsl:variable name="propertyAlias" select="string('blogroll')"/>
  
  <xsl:template match="/">
    <!-- The fun starts here -->
    <xsl:if test="count($currentPage/ancestor-or-self::umbBlog/* [name() = $propertyAlias]/links/link) &gt; 0">
      <li id="linkcat-2" class="linkcat">
        <h3>Blogroll</h3>
        
        <ul>
          <xsl:for-each select="$currentPage/ancestor-or-self::umbBlog/* [name() = $propertyAlias]/links/link">
            <li>
              <xsl:element name="a">
                <xsl:if test="./@newwindow = '1'">
                  <xsl:attribute name="target">_blank</xsl:attribute>
                </xsl:if>
                <xsl:choose>
                  <xsl:when test="./@type = 'external'">
                    <xsl:attribute name="href">
                      <xsl:value-of select="./@link"/>
                    </xsl:attribute>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="href">
                      <xsl:value-of select="umbraco.library:NiceUrl(./@link)"/>
                    </xsl:attribute>
                  </xsl:otherwise>
                </xsl:choose>
                <xsl:value-of select="./@title"/>
              </xsl:element>
            </li>
          </xsl:for-each>
        </ul>

      </li>
    </xsl:if>
    
  </xsl:template>

</xsl:stylesheet>
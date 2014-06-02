<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [
  <!ENTITY nbsp "&#x00A0;">
]>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxml="urn:schemas-microsoft-com:xslt"
  xmlns:umbraco.library="urn:umbraco.library" xmlns:Exslt.ExsltCommon="urn:Exslt.ExsltCommon" xmlns:Exslt.ExsltDatesAndTimes="urn:Exslt.ExsltDatesAndTimes" xmlns:Exslt.ExsltMath="urn:Exslt.ExsltMath" xmlns:Exslt.ExsltRegularExpressions="urn:Exslt.ExsltRegularExpressions" xmlns:Exslt.ExsltStrings="urn:Exslt.ExsltStrings" xmlns:Exslt.ExsltSets="urn:Exslt.ExsltSets" xmlns:tagsLib="urn:tagsLib" xmlns:BlogLibrary="urn:BlogLibrary"
  exclude-result-prefixes="msxml umbraco.library Exslt.ExsltCommon Exslt.ExsltDatesAndTimes Exslt.ExsltMath Exslt.ExsltRegularExpressions Exslt.ExsltStrings Exslt.ExsltSets tagsLib BlogLibrary ">


  <xsl:output method="xml" omit-xml-declaration="yes"/>

  <xsl:param name="currentPage"/>

  <xsl:variable name="numberOfItems">
    <xsl:choose>
      <xsl:when test="/macro/numberOfItems != ''">
        <xsl:value-of select="/macro/numberOfItems"/>
      </xsl:when>
      <xsl:otherwise>5</xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <xsl:variable name="excerptLength">
    <xsl:choose>
      <xsl:when test="string(/macro/excerptLength) != ''">
        <xsl:value-of select="/macro/excerptLength"/>
      </xsl:when>
      <xsl:otherwise>100</xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

  <xsl:template match="/">

    <h3>Latest news</h3>
    <ul class="summaryList" id="newsSummary">
      <xsl:for-each select="$currentPage/umbNewsArea/umbNewsArticle [@isDoc and string(umbracoNaviHide) != '1']">
        <xsl:if test="position() &lt;= $numberOfItems">
          <li>
            <h4>
              <a href="{umbraco.library:NiceUrl(@id)}">
                <xsl:value-of select="@nodeName"/>
              </a>
            </h4>
            <p>
              <xsl:choose>
                <xsl:when test="string($excerptLength) != '0'">
                  <xsl:value-of select="umbraco.library:TruncateString(umbraco.library:StripHtml(introduction), number($excerptLength), '...')" disable-output-escaping="yes"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="introduction" disable-output-escaping="yes"/>
                </xsl:otherwise>
              </xsl:choose>
            </p>
            <small>
              <xsl:value-of select="umbraco.library:ShortDate(@createDate)"/>
            </small>
          </li>
        </xsl:if>
      </xsl:for-each>
    </ul>
  </xsl:template>

</xsl:stylesheet>
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

    <xsl:template match="/">
        <div class="tagcloud">
            <p>

                <xsl:for-each select="tagsLib:getAllTags()/tags/tag">
                    <xsl:sort select="." order="ascending"/>
                    <a href="{umbraco.library:NiceUrl($currentPage/ancestor-or-self::umbBlog/@id)}?filterby={.}">
                        <xsl:attribute name="class">
                            <xsl:choose>
                                <xsl:when test="@nodesTagged &gt; 5">
                                    <xsl:value-of select="string('tagweight5')"  />
                                </xsl:when>
                                <xsl:otherwise>
                                    <xsl:value-of select="concat('tagweight',@nodesTagged)"/>
                                </xsl:otherwise>
                            </xsl:choose>
                        </xsl:attribute>
                        <xsl:value-of select="."/>
                    </a>
                    <xsl:text> </xsl:text>
                </xsl:for-each>

            </p>
        </div>

    </xsl:template>

</xsl:stylesheet>
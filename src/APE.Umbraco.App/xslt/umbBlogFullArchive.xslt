<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:msxml="urn:schemas-microsoft-com:xslt"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
xmlns:math="http://exslt.org/math"
xmlns:umbraco.library="urn:umbraco.library"
exclude-result-prefixes="msxml umbraco.library">


    <xsl:output method="html" omit-xml-declaration="yes"/>

    <xsl:param name="currentPage"/>
   
    <xsl:template match="/">
        <div id="archieve">

            <xsl:for-each select="$currentPage/ancestor-or-self::umbBlog/DateFolder">
                <xsl:sort select="number(@nodeName)" data-type="number" order="descending"/>
                <h3>
                    <xsl:value-of select="@nodeName"/>
                </h3>
                <div class="tab">
                    <xsl:for-each select="./DateFolder">
                        <xsl:sort select="number(@nodeName)" data-type="number" order="descending"/>
                        <h4>
                            <xsl:value-of select="umbraco.library:FormatDateTime( concat('01-', @nodeName , '-2010'), 'MMMM' )"/>
                        </h4>
                        <ul>
                            <xsl:for-each select=".//umbBlogPost">
                                <li>
                                    <a href="{umbraco.library:NiceUrl(@id)}">
                                        <xsl:value-of select="@nodeName"/>
                                    </a>
                                    <br/>
                                    <small>
                                        Posted: <xsl:value-of select="umbraco.library:LongDate(PostDate)"/>
                                        By: <xsl:value-of select="@writerName"/>
                                    </small>
                                </li>

                            </xsl:for-each>
                        </ul>
                    </xsl:for-each>
                </div>

            </xsl:for-each>
        </div>

    </xsl:template>

</xsl:stylesheet>
<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output encoding="UTF-8" indent="yes" method="html" standalone="no" omit-xml-declaration="no"/>
	
	<!-- Copy of all attributes -->
	<xsl:template match="node()|@*">
		<xsl:copy>
			<xsl:apply-templates select="node()|@*"/>
		</xsl:copy>
	</xsl:template>
	<!-- Read  Bool -->
	<xsl:template match="Read/Parameters">	
	<xsl:variable name="expectedResult">
	<xsl:value-of select="./expectedResult"/>
	</xsl:variable>
	<xsl:choose>
	<xsl:when test="expectedResult!=''">
	<ExpectedResult>
	<xsl:value-of select="./expectedResult"/>
	</ExpectedResult>
	</xsl:when>
	<xsl:otherwise>
	</xsl:otherwise>
	</xsl:choose>
	</xsl:template>
	<!-- Read SelectEntitiesBox  -->
	<xsl:template match="SelectEntitiesBox/Actions/Read/Parameters">
	    <Actions>
		 <xsl:for-each select = "expectedResult">
		 <xsl:variable name="i" select="position()-1" />
			<ElementAt>
			<Parameters>
			<index><xsl:value-of select="$i"/></index>
			</Parameters>
         <ExpectedResult>
		 <xsl:value-of select="text()"/>
        </ExpectedResult>
		</ElementAt>
		</xsl:for-each>
		</Actions>
	</xsl:template>
	
	<!-- Read  -->
	<xsl:template match="Read/ExpectedResult">
         <ExpectedResult>
			<xsl:value-of select="./value"/>
        </ExpectedResult>
	</xsl:template>
		
	<!-- AttributeName  -->
	<xsl:template match="AttributeName/ExpectedResult">
         <ExpectedResult>
			<xsl:value-of select="./value"/>
        </ExpectedResult>
	</xsl:template>
	
	<!-- ReadBanner  -->
	<xsl:template match="ReadBanner/Parameters">
         <ExpectedResult>
			<xsl:value-of select="./objectName"/>
        </ExpectedResult>
	</xsl:template>
	<!-- ReadV2  -->
	<xsl:template match="ReadV2/Parameters">
         <ExpectedResult>
			<xsl:value-of select="./expectedObjectType"/>
        </ExpectedResult>
	</xsl:template>	
	<!-- ReadValue, There is 3 types of ReadValue -->
	<xsl:template match="ReadValue/Parameters">
	<xsl:variable name="expectedResult">
	<xsl:value-of select="./expectedResult"/>
	</xsl:variable>	
		<Parameters>
		<xsl:copy-of select="./colIndex"/>
		<xsl:copy-of select="./rowIndex"/>
		<!-- OR -->
		<xsl:copy-of select="./Row"/>
		<xsl:copy-of select="./Column"/>
		</Parameters>
		
	<xsl:choose>
	<xsl:when test="expectedResult!=''">
	    <ExpectedResult>
			<xsl:value-of select="./expectedResult"/>
        </ExpectedResult>
	</xsl:when>
	<xsl:otherwise>
	</xsl:otherwise>
	</xsl:choose>		
	</xsl:template>
		<!-- ReadValue ExpectedResult-->
	<xsl:template match="ReadValue/ExpectedResult">
         <ExpectedResult>
			<xsl:value-of select="./value"/>
        </ExpectedResult>
	</xsl:template>
	<!-- ReadComment ExpectedResult-->
	<xsl:template match="ReadComment/ExpectedResult">
         <ExpectedResult>
			<xsl:value-of select="./value"/>
        </ExpectedResult>
	</xsl:template>	
	
	<!-- ReadTab  -->
	<xsl:template match="ReadTab/ExpectedResult">
         <ExpectedResult>
			<xsl:value-of select="./value"/>
        </ExpectedResult>
	</xsl:template>
	
	<!-- ReadUrlNewTab  -->
	<xsl:template match="ReadUrlNewTab/Parameters">
         <ExpectedResult>
			<xsl:value-of select="./expectedUrl"/>
        </ExpectedResult>
	</xsl:template>
	
	<!-- ReadTabId  -->
	<xsl:template match="ReadTabId/Parameters">
         <ExpectedResult>
			<xsl:value-of select="./TabId"/>
        </ExpectedResult>
	</xsl:template>
	
	
	<!--ReadHeader -->
	<xsl:template match="ReadHeader/ExpectedResult">
         <ExpectedResult>
			<xsl:value-of select="./value"/>
        </ExpectedResult>
	</xsl:template>
</xsl:stylesheet>

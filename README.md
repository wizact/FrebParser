FrebParser
==========

A handy console application to parse all the FREB logs in a directory and persist data in database. I created this application for my monthly check on my web server errors. Usually I parse around few thousands of FREB files and persist them in the database. In that way I can query and group data to find out the issues happened during the month. 

**This application is provided as-is. There are some tweaks in there for the purpose of getting information required and removing trivial errors.**

Usage:
  1) Configure the connection string in config file
  2) Use Visual Studio 2010 to build the project
  3) Use the following command to parse files in a directory:
      
      FrebParserConsole.exe "c:\Path\to\directory\"

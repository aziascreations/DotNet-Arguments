@echo off

cd %~dp0

del index.md
copy "..\readme.md" index.md

del building.md
copy "..\building.md" building.md

rmdir /S /Q _site
rmdir /S /Q obj\.cache

docfx build ./docfx.json

del _site\api\index.html
copy "_site\api\NibblePoker.Library.Arguments.html" "_site\api\index.html"

docfx serve _site/

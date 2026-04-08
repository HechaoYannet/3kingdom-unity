.\tabtoy.exe ^
--mode=v2 ^
--json_out=..\Assets\StreamingAssets\JsonData\Character.json ^
--combinename=Character ^
--lan=zh_cn ^
DataConfig\Character.xlsx

@IF %ERRORLEVEL% NEQ 0 pause

pause
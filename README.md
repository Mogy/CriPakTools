# CriPakTools for DBX2_JP_MOD_TOOL

CPKファイルからデータ抽出を行うツールCriPakToolsを[DBX2_JP_MOD_TOOL](https://github.com/Mogy/DBX2_JP_MOD_TOOL)用に改変したもの

抽出するファイルを正規表現で指定する事が出来る

# 使い方

## コマンド
```
CriPakTool.exe IN_FILE EXTRACT_ME
```
* IN_FILE: CPKファイル
* EXTRACT_ME: 抽出するファイル(正規表現)

## 例
```
CriPakTool.exe cpk/data1.cpk ^*.\.msg$
```
data1.cpkから*.msgファイルを全て抽出する

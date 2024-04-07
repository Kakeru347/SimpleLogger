# SimpleLogger

SimpleLoggerは、ログの出力時間とメッセージだけを記録するシンプルなロガーです。
ジョブの実行、タスクの自動化ログ出力など、シンプルなログ記録が必要な処理に最適です。

SimpleLogger is a simple logger that only logs output times and messages.
It is ideal for processes that require simple logging, such as logging of batch process execution.

## 使い方 / How Use

### インストール / Install

このライブラリはソースコードベースです。GitHubからクローンまたはダウンロードし、プロジェクトに追加してください。

This library is source code based, so you can clone it from GitHub or download it and add it to your project.

### 基本的な使用方法 / Use

```csharp
using System;

// Loggerインスタンスの作成。ログファイルのパスとローテーションサイズはオプションです。
// Create logger instance.
// Log file path and rotation size are optional.
var logger = new Logger(filePath, rotateSize);

// ログレベルを設定してメッセージをログに書き込む。
// Set the log level and write messages to the log.
logger.Info("これはINFOログです");
logger.Error("これはERRORログです");

```

### ログレベルの設定 / Log Level Settings

ログレベルを変更することで、出力する最小のログレベルを制御できます。

The minimum log level to be output can be controlled by changing the log level.

```csharp
logger.MinLogLevel = Logger.LogLevel.DEBUG;
```

### ログの出力例 / Example of log output

```log
2024/04/07 20:53:11.547	[TRACE]	これはTRACEログです
2024/04/07 20:53:11.552	[DEBUG]	これはDEBUGログです
2024/04/07 20:53:11.552	[INFO]	これはINFOログです
2024/04/07 20:53:11.552	[WARNING]	これはWARNINGログです
2024/04/07 20:53:11.552	[ERROR]	これはERRORログです
2024/04/07 20:53:11.552	[FATAL]	これはFATALログです
```

using System.Text;

public class Logger : IDisposable
{
    private string logFilePath;
    private StreamWriter logFileWriter;
    private LogLevel minLogLevel = LogLevel.INFO;
    private const int defultRotateSize = 16 * 1024 * 1024;  // 16MB
    private int rotateSize = defultRotateSize;

    public enum LogLevel
    {
        TRACE
        , DEBUG
        , INFO
        , WARNING
        , ERROR
        , FATAL
    }

    /// <summary>
    /// 出力する最小のログレベルを取得または設定します。
    /// 設定したログレベル以上が出力されます。
    /// 設定しない場合は、「INFO」がデフォルト値になります。
    /// </summary>
    public LogLevel MinLogLevel
    {
        get { return minLogLevel; }
        set { minLogLevel = value; }
    }

    /// <summary>
    /// Loggerクラスのインスタンスを初期化します。
    /// ログファイルパスとローテーションサイズをオプションで設定できます。
    /// </summary>
    /// <param name="filePath">ログファイルのパス（拡張子を含む）。指定しない場合は、デフォルトパスが使用されます。</param>
    /// <param name="inRotateSize">ログファイルのローテーションサイズ。指定しない場合は、デフォルトサイズ（16MB）が使用されます。</param>
    public Logger(string filePath = null, int inRotateSize = defultRotateSize)
    {
        logFilePath = filePath ?? GetDefaultLogFilePath();
        rotateSize = inRotateSize;
        CheckAndRotateLogFile();
    }

    // 実行ディレクトリに「log\yyyy\MM.log」でログファイルを指定します。
    private static string GetDefaultLogFilePath()
    {
        var now = DateTime.Now;
        string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log", now.Year.ToString());
        Directory.CreateDirectory(logDirectory);
        return Path.Combine(logDirectory, now.ToString("MM") + ".log");
    }

    private void CheckAndRotateLogFile()
    {
        if (File.Exists(logFilePath) && new FileInfo(logFilePath).Length > rotateSize)
        {
            RotateLog(logFilePath);
        }
    }

    // 古いログを「MM.log」から「MM_yyyyMMddHHmmss.log」にローテーションします。
    private static void RotateLog(string logFilePath)
    {
        string logFileName = Path.GetFileNameWithoutExtension(logFilePath);
        string logFileExtension = Path.GetExtension(logFilePath);
        string logFileDirectory = Path.GetDirectoryName(logFilePath);
        string rotationDate = DateTime.Now.ToString("yyyyMMddHHmmss");
        string newLogFilePath = Path.Combine(logFileDirectory, $"{logFileName}_{rotationDate}{logFileExtension}");

        File.Move(logFilePath, newLogFilePath);
    }

    /// <summary>
    /// メッセージを指定されたログレベルでログに書き込みます。
    /// </summary>
    /// <param name="message">ログに書き込むメッセージ。</param>
    /// <param name="level">メッセージのログレベル。</param>
    private void Log(string message, LogLevel level)
    {
        if (level < minLogLevel) return;

        try
        {
            if (logFileWriter == null)
            {
                logFileWriter = new StreamWriter(logFilePath, true, Encoding.UTF8);
            }

            string formattedMessage = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}\t[{level}]\t{message}";
            logFileWriter.WriteLine(formattedMessage);
            logFileWriter.Flush();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ログ書き込みエラー: {ex.Message}");
        }
    }

    // ログレベル毎にログ書き込みメソッド
    public void Trace(string message) => Log(message, LogLevel.TRACE);
    public void Debug(string message) => Log(message, LogLevel.DEBUG);
    public void Info(string message) => Log(message, LogLevel.INFO);
    public void Warning(string message) => Log(message, LogLevel.WARNING);
    public void Error(string message) => Log(message, LogLevel.ERROR);
    public void Fatal(string message) => Log(message, LogLevel.FATAL);

    public void Dispose()
    {
        logFileWriter?.Dispose();
    }
}

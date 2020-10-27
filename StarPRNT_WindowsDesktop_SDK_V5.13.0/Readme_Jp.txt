************************************************************
      StarPRNT SDK Ver 5.13.0
         Readme_Jp.txt                  スター精密（株）
************************************************************

    1. 概要
    2. Ver5.13.0についての変更点
    3. 内容
    4. 適用
    5. 注意事項
    6. 著作権
    7. 変更履歴

==============================
 1. 概要
==============================

    本パッケージは、StarPRNT SDK Ver 5.13.0 です。
    StarIOPort/StarIO/StarIOExtension/SMCloudServicesSolution/GenerateBarcodeは、
    Starプリンタを使用するアプリケーションの開発を容易にするためのライブラリです。

    詳細な説明は、SDKドキュメントファイルを参照ください。

==============================
 2. Ver5.13.0についての変更点
==============================

    [SDK]
        - プロジェクト構成の変更
            * 各ライブラリのNuget対応

    [マニュアル]
        - StarPRNT SDKユーザーズマニュアルをPDFからオンラインマニュアルへのリンクに変更

==============================
 3. 内容
==============================

    StarPRNT_WindowsDesktop_SDK_V5_13_0
    |- Readme_En.txt                                // リリースノート (英語)
    |- Readme_Jp.txt                                // リリースノート (日本語)
    |- SoftwareLicenseAgreement_En.pdf              // ソフトウエア使用許諾書 (英語)
    |- SoftwareLicenseAgreement_Jp.pdf              // ソフトウエア使用許諾書 (日本語)
    |
    +- Documents
    |  |- UsersManual.url                           // StarPRNT SDK ドキュメントへのリンク
    |  |- CommandEmulator_on_SMCS_En.pdf            // SMCSコマンドエミュレータのコマンド対応リスト (英語)
    |  +- CommandEmulator_on_SMCS_Jp.pdf            // SMCSコマンドエミュレータのコマンド対応リスト (日本語)
    |
    +- Software
    |  |- SDK                                       // サンプルプログラム (Ver 5.13.0)
    |  |- Distributables                            // 厳密な名前なしライブラリ
    |  |   |- StarIOPort_x86.dll                    // StarIOPort_x86.dll          (Ver 2.6.0 32bitアプリケーション用)
    |  |   |- StarIOPort_x64.dll                    // StarIOPort_x64.dll          (Ver 2.6.0 64bitアプリケーション用)
    |  |   |- GenerateBarcode_x86.dll               // GenerateBarcode_x86.dll     (Ver 1.0.0 32bitアプリケーション用)
    |  |   |- GenerateBarcode_x64.dll               // GenerateBarcode_x64.dll     (Ver 1.0.0 64bitアプリケーション用)
    |  |   |- StarIO.dll                            // StarIO.dll                  (Ver 2.6.0)
    |  |   |- StarIOExtension.dll                   // StarIOExtension.dll         (Ver 1.6.0)
    |  |   +- SMCloudServicesSolution.dll           // SMCloudServicesSolution.dll (Ver 1.1.1)
    |  |
    |  +- Distributables_strong_named               // 厳密な名前付きライブラリ
    |      |- StarIOPort_x86.dll                    // StarIOPort_x86.dll          (Ver 2.6.0 32bitアプリケーション用)
    |      |- StarIOPort_x64.dll                    // StarIOPort_x64.dll          (Ver 2.6.0 64bitアプリケーション用)
    |      |- GenerateBarcode_x86.dll               // GenerateBarcode_x86.dll     (Ver 1.0.0 32bitアプリケーション用)
    |      |- GenerateBarcode_x64.dll               // GenerateBarcode_x64.dll     (Ver 1.0.0 64bitアプリケーション用)
    |      |- StarIO.dll                            // StarIO.dll                  (Ver 2.6.0)
    |      |- StarIOExtension.dll                   // StarIOExtension.dll         (Ver 1.6.0)
    |      +- SMCloudServicesSolution.dll           // SMCloudServicesSolution.dll (Ver 1.1.1)
    |
    +- Others
        |- PrinterSoftwareRecoveryTool              // プリンタドライバソフトウェアアップデート用ツール (Ver 1.0.0)
        +- StarSoundConverter                       // メロディスピーカー向け音声変換ツール (Ver 1.0.0)

==============================
 4. 適用
==============================

    対応エミュレーションについては以下になります。
        - StarPRNT
        - Star Line Mode
        - Star Graphic Mode
        - ESC/POS
        - ESC/POS Mobile
        - Star Dot Impact

    コマンドの詳細は各コマンド仕様書を参照ください。
    各マニュアルはスター精密のウェブサイトで入手可能です。

==============================
 5. 注意事項
==============================

    1. Ver2.2以前のWindows Desktop UI用SDK同梱の'StarIOPort.dll'または'StarIO.dll'を
        本パッケージ同梱のものに差し替える場合は以下の点にご注意ください。

        - 以下の環境を満たしていない場合はアプリケーションがビルドできません。
            OS                       : Windows7, 8/8.1, 10
            Visual Studio            : Visual Studio 2008以降
            ターゲットフレームワーク : .Net Framework 3.5以上

        - モバイルプリンタ用SDKをご利用されていた場合、'GetPort'メソッドの引数を変更する必要があります。
        お手数ですが、本パッケージ同梱のStarPRNT SDKドキュメントをご参照の上、
        'GetPort'メソッドの引数を再設定し、動作確認を行ってください。

    2. プリンターの電源をONした後にプリンターにmC-Soundを接続した場合、メロディスピーカーのAPIは正常に動作しません。
        プリンターにmC-Soundを接続した後にプリンターの電源をONしてください。

    3. ReleasePortメソッドの挙動の変更
        - StarIO.dll V2.6.0 (StarPRNT SDK V5.11.0)より、ReleasePortメソッドの挙動を変更しました。
            V2.5.0以前:
                ReleasePortメソッドを呼ぶ前にGetPortメソッドを複数回呼んだ場合は、
                GetPortメソッドを呼んだ回数分ReleasePortメソッドを呼ぶことでポートがクローズされる。
            V2.6.0以降:
                ReleasePortメソッドを呼んだ場合は、GetPortメソッドを呼んだ回数に関わらず必ずポートがクローズされる。

==============================
 6. 著作権
==============================

    スター精密（株）Copyright 2019

==============================
 7. 変更履歴
==============================

    Ver. 5.13.0
    2019/12/25
        [SDK]
            - プロジェクト構成の変更
                * 各ライブラリのNuget対応

        [マニュアル]
            - StarPRNT SDKユーザーズマニュアルをPDFからオンラインマニュアルへのリンクに変更

    Ver. 5.11.0
    2019/09/04
        [StarIO]
        - 機能追加
            * PortExceptionクラスにErrorCodeプロパティを追加
        - 仕様変更
            * ReleasePortメソッドの挙動を変更
            変更前:
                ReleasePortメソッドを呼ぶ前にGetPortメソッドを複数回呼んだ場合は、
                GetPortメソッドを呼んだ回数分ReleasePortメソッドを呼ぶことでポートがクローズされる。
            変更後:
                ReleasePortメソッドを呼んだ場合は、GetPortメソッドを呼んだ回数に関わらず必ずポートがクローズされる。

        [StarIOExtension]
            - 機能追加
                * AppendBlackMarkメソッドでStarPRNTエミュレーションのモバイルプリンタのブラックマーク設定の有効/無効を切り替えできるように変更

        [SMCloudServicesSolution]
            - バグ修正
                * ShowRegistrationViewメソッドで表示されるビューのレイアウトの軽微な修正

        [SDK]
            - サンプルコードの追加
                * プリンタとの通信失敗要因取得の実装例

    Ver. 5.10.0
    2019/03/07
        [StarIO]
            - 機能追加
                * GetPortメソッドのportNameパラメータにBDアドレスを指定できる機能を追加

        [StarIOExtension]
            - 機能追加
                * AppendCjkUnifiedIdeographFontメソッドを追加
                UTF-8におけるCJK統合漢字フォントの優先順位を指定可能
                (TSP650II JP2/TWモデル ファームウェアバージョン4.0以降、mC-Print3、mC-Print2のみ対応)
                * BarcodeWidth列挙体にコマンド仕様に準拠したバーコードモード（シンボル幅）が指定されるExtModeを追加
            - バグ修正
                * ESC/POS Mobileエミュレーションで、印字されるイメージがAppendBitmapWithAbsolutePositionメソッド
                で指定したpositionの2倍絶対位置移動される問題を修正
                * ページモード時、AppendBitmapWithAbsolutePositionメソッドでpositionに0を指定しても、
                横軸の印字開始位置が行頭に設定されない問題を修正
                * StarPRNTエミュレーションで、AppendBitmapメソッドで入力したビットマップ下端の空白が印字されない問題を修正

        [SDK]
            - サンプルコードの変更
                * UTF-8におけるCJK統合漢字印字サンプルの実装例をAppendCjkUnifiedIdeographFontメソッドを
                使用するよう変更

    Ver. 5.9.0
    2018/11/20
        [StarIOExtension]
            - 機能追加
                * メロディスピーカーのAPI対応

        [SDK]
            - サンプルコードの追加
                * メロディスピーカー制御の実装例

    Ver. 5.7.0
    2018/06/29
        [StarIO]
            - バグ修正
                * 特定の環境においてSearchPrinterメソッドでEthernetプリンターを検出できない問題を修正

        [StarIOExtension]
            - 以下APIのサポート終了
                * StarIoExt.CreateScaleCommandBuilderメソッド
                * StarIoExt.CreateScaleConnectParserメソッド
                * StarIoExt.CreateScaleWeightParserメソッド
                * ScaleModel列挙体
                * IScaleCommandBuilderインターフェイス
                * IPeripheralCommandParser.ReceiveCommandsプロパティ
                * IScaleWeightParserインターフェイス

        [SMCloudServicesSolution]
            - 機能追加
                * SMCloudServices.ShowRegistrationWindow メソッド
                SCSダッシュボードのパスワードを変更した場合に、ローカルのパスワードを更新出来るようにしました。

        [SDK]
            - サンプルコードの追加
                * 迂回印刷の実装例

    Ver. 5.6.0
    2018/05/21
        [StarIO]
            - 機能追加
                * mC-Print2, mC-Print3に対応
                * GetPortメソッドのportSettingsパラメータに指定できるオプションを追加
                * StarPrinterStatusクラスにConnectedInterfaceプロパティを追加
            - バグ修正
                * 特定の環境においてSearchPrinterメソッドでBluetoothプリンターを検出できない問題を修正
                * SearchPrinterメソッドでUSBシリアルナンバーを取得できない場合がある問題を修正
            - 厳密な名前付きのライブラリを追加

        [StarIOExtension]
            - 機能追加
                * トップマージンを設定するメソッドを追加
                * 印字領域を設定するメソッドを追加
                *AppendSoundメソッドに駆動時間、ディレイ時間を設定するパラメータを追加
            - バグ修正
                * IScaleWeightParserインターフェイスのParseメソッドが適切な結果を返さない場合がある問題を修正
                * StarPrintPortJobMonitorクラスのStopメソッドが適切に終了しない場合がある問題を修正
            - 厳密な名前付きのライブラリを追加

        [SMCloudServicesSolution]
            - 厳密な名前付きのライブラリを追加

        [SDK]
            - サンプルコードの追加
                * USBシリアルナンバーを設定するサンプルコードの追加

    Ver. 5.5.0
    2018/01/31
        [StarIO]
            - ステータス監視中に、LANインターフェイスのプリンタ電源オフ後、
            タイムアウト時間内にプリンタが復帰すると、印字できなくなる場合がある問題を修正

        [StarIOExtension]
            - 水平タブを設定・クリアするメソッドを追加

        [SDK]
            - サンプルコードの追加
                * UTF-8におけるCJK統合漢字 印字サンプルの実装例
                (TSP650II JP2/TWモデル ファームウェアバージョン4.0以降のみ対応)

    Ver. 5.4.1
    2017/10/26
        ライブラリ難読化ライセンスの更新

    Ver. 5.4.0
    2017/10/12
        初版リリース

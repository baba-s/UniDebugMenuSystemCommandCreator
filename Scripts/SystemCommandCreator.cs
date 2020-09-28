using System;
using System.Linq;
using UnityEngine;

namespace Kogane.DebugMenu
{
	/// <summary>
	/// システムコマンドのリストを作成するクラス
	/// </summary>
	public sealed class SystemCommandCreator : ListCreatorBase<CommandData>
	{
		//==============================================================================
		// 列挙型
		//==============================================================================
		private enum TabType
		{
			NONE,
			APPLICATION,
			DEBUG,
			SCREEN,
			TIME,
		}

		//==============================================================================
		// 変数(SerializeField)
		//==============================================================================
		private readonly CommandDataWithTabType<TabType>[] m_sourceList;

		//==============================================================================
		// 変数
		//==============================================================================
		private CommandData[] m_list;

		//==============================================================================
		// プロパティ
		//==============================================================================
		public override int  Count       => m_list.Length;
		public override bool IsShowToast => true;

		public override string[] TabNameList =>
			new[]
			{
				"All",
				"Application",
				"Debug",
				"Screen",
				"Time",
			};

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// 作成して返します
		/// </summary>
		public SystemCommandCreator()
		{
			var creator = new CommandDataCreator<TabType>();

			m_sourceList = new[]
			{
				creator.Create
				(
					TabType.APPLICATION,
					() => "APPLICATION"
				),
				creator.Create
				(
					TabType.APPLICATION,
					() => $"FPS: {Application.targetFrameRate}",
					new InputActionData( InputValidateType.INTEGER, str => Application.targetFrameRate = int.Parse( str ) ),
					new ActionData( "30", () => Application.targetFrameRate                            = 30 ),
					new ActionData( "60", () => Application.targetFrameRate                            = 60 )
				),
				creator.Create
				(
					TabType.DEBUG,
					() => "DEBUG"
				),
				creator.Create
				(
					TabType.DEBUG,
					() => $"ログ出力: {Debug.unityLogger.logEnabled}",
					new ToggleActionData( () => Debug.unityLogger.logEnabled, isOn => Debug.unityLogger.logEnabled = isOn )
				),
				creator.Create
				(
					TabType.DEBUG,
					() => $"Log: {Application.GetStackTraceLogType( LogType.Log )}",
					new ActionData( "None", () => Application.SetStackTraceLogType( LogType.Log, StackTraceLogType.None ) ),
					new ActionData( "ScriptOnly", () => Application.SetStackTraceLogType( LogType.Log, StackTraceLogType.ScriptOnly ) ),
					new ActionData( "Full", () => Application.SetStackTraceLogType( LogType.Log, StackTraceLogType.Full ) )
				),
				creator.Create
				(
					TabType.DEBUG,
					() => $"Warning: {Application.GetStackTraceLogType( LogType.Warning )}",
					new ActionData( "None", () => Application.SetStackTraceLogType( LogType.Warning, StackTraceLogType.None ) ),
					new ActionData( "ScriptOnly", () => Application.SetStackTraceLogType( LogType.Warning, StackTraceLogType.ScriptOnly ) ),
					new ActionData( "Full", () => Application.SetStackTraceLogType( LogType.Warning, StackTraceLogType.Full ) )
				),
				creator.Create
				(
					TabType.DEBUG,
					() => $"Error: {Application.GetStackTraceLogType( LogType.Error )}",
					new ActionData( "None", () => Application.SetStackTraceLogType( LogType.Error, StackTraceLogType.None ) ),
					new ActionData( "ScriptOnly", () => Application.SetStackTraceLogType( LogType.Error, StackTraceLogType.ScriptOnly ) ),
					new ActionData( "Full", () => Application.SetStackTraceLogType( LogType.Error, StackTraceLogType.Full ) )
				),
				creator.Create
				(
					TabType.DEBUG,
					() => $"Assert: {Application.GetStackTraceLogType( LogType.Assert )}",
					new ActionData( "None", () => Application.SetStackTraceLogType( LogType.Assert, StackTraceLogType.None ) ),
					new ActionData( "ScriptOnly", () => Application.SetStackTraceLogType( LogType.Assert, StackTraceLogType.ScriptOnly ) ),
					new ActionData( "Full", () => Application.SetStackTraceLogType( LogType.Assert, StackTraceLogType.Full ) )
				),
				creator.Create
				(
					TabType.DEBUG,
					() => $"Exception: {Application.GetStackTraceLogType( LogType.Exception )}",
					new ActionData( "None", () => Application.SetStackTraceLogType( LogType.Exception, StackTraceLogType.None ) ),
					new ActionData( "ScriptOnly", () => Application.SetStackTraceLogType( LogType.Exception, StackTraceLogType.ScriptOnly ) ),
					new ActionData( "Full", () => Application.SetStackTraceLogType( LogType.Exception, StackTraceLogType.Full ) )
				),
				creator.Create
				(
					TabType.DEBUG,
					() => "不具合",
					new ActionData( "エラーログ", () => Debug.LogError( "エラー発生テスト" ) ),
					new ActionData( "例外", () => Debug.LogException( new Exception( "例外発生テスト" ) ) )
				),
				creator.Create
				(
					TabType.DEBUG,
					() => "無限ループ",
					new ActionData
					(
						"while", () =>
						{
							while ( true )
							{
							}
						}
					),
					new ActionData
					(
						"for", () =>
						{
							for ( ;; )
							{
							}
						}
					)
				),
				creator.Create
				(
					TabType.SCREEN,
					() => "SCREEN"
				),
				creator.Create
				(
					TabType.SCREEN,
					() => $"スリープするかどうか: {Screen.sleepTimeout == SleepTimeout.SystemSetting}",
					new ActionData( "しない", () => Screen.sleepTimeout = SleepTimeout.NeverSleep ),
					new ActionData( "する", () => Screen.sleepTimeout  = SleepTimeout.SystemSetting )
				),
				creator.Create
				(
					TabType.SCREEN,
					() => $"端末の向き: {Screen.orientation}",
					new ActionData( "自動回転", () => Screen.orientation     = ScreenOrientation.AutoRotation ),
					new ActionData( "縦持ち", () => Screen.orientation      = ScreenOrientation.Portrait ),
					new ActionData( "縦持ち\n（逆）", () => Screen.orientation = ScreenOrientation.PortraitUpsideDown )
				),
				creator.Create
				(
					TabType.SCREEN,
					() => "",
					new ActionData( "横持ち", () => Screen.orientation       = ScreenOrientation.Landscape ),
					new ActionData( "横持ち\n（左下）", () => Screen.orientation = ScreenOrientation.LandscapeLeft ),
					new ActionData( "横持ち\n（右下）", () => Screen.orientation = ScreenOrientation.LandscapeRight )
				),
				creator.Create
				(
					TabType.TIME,
					() => "TIME"
				),
				creator.Create
				(
					TabType.TIME,
					() => $"タイム\nスケール: {Time.timeScale}",
					new InputActionData( str => Time.timeScale  = float.Parse( str ) ),
					new ActionData( "0", () => Time.timeScale   = 0 ),
					new ActionData( "0.5", () => Time.timeScale = 0.5f ),
					new ActionData( "1", () => Time.timeScale   = 1 ),
					new ActionData( "2", () => Time.timeScale   = 2 ),
					new ActionData( "4", () => Time.timeScale   = 4 )
				),
				creator.Create
				(
					TabType.NONE,
					() => "OTHER"
				),
				creator.Create
				(
					TabType.NONE,
					() => "Resources.UnloadUnusedAssets",
					new ActionData( "実行", () => Resources.UnloadUnusedAssets() )
				),
				creator.Create
				(
					TabType.NONE,
					() => "GC.Collect",
					new ActionData( "実行", () => GC.Collect() )
				),
				creator.Create
				(
					TabType.NONE,
					() => "Caching.ClearCache",
					new ActionData( "実行", () => Caching.ClearCache() )
				),
				creator.Create
				(
					TabType.NONE,
					() => "トースト表示テスト",
					new ActionData( "実行", () => { } )
				),
				creator.Create
				(
					TabType.NONE,
					() => "ForceCrash",
					new ActionData( "AccessViolation", () => UnityEngine.Diagnostics.Utils.ForceCrash( UnityEngine.Diagnostics.ForcedCrashCategory.AccessViolation ) ),
					new ActionData( "FatalError", () => UnityEngine.Diagnostics.Utils.ForceCrash( UnityEngine.Diagnostics.ForcedCrashCategory.FatalError ) ),
					new ActionData( "Abort", () => UnityEngine.Diagnostics.Utils.ForceCrash( UnityEngine.Diagnostics.ForcedCrashCategory.Abort ) ),
					new ActionData( "PureVirtualFunction", () => UnityEngine.Diagnostics.Utils.ForceCrash( UnityEngine.Diagnostics.ForcedCrashCategory.PureVirtualFunction ) )
				),
			};
		}

		/// <summary>
		/// リストの表示に使用するデータを作成します
		/// </summary>
		protected override void DoCreate( ListCreateData data )
		{
			var tabType = ( TabType ) data.TabIndex;
			var isAll   = tabType == 0;

			m_list = m_sourceList
					.Where( c => isAll || c.TabType == tabType )
					.Select( c => c.Data )
					.Where( c => data.IsMatch( c.GetText() ) )
					.ToArray()
				;

			if ( data.IsReverse )
			{
				Array.Reverse( m_list );
			}
		}

		/// <summary>
		/// 指定されたインデックスの要素の表示に使用するデータを返します
		/// </summary>
		protected override CommandData DoGetElemData( int index )
		{
			return m_list.ElementAtOrDefault( index );
		}
	}
}
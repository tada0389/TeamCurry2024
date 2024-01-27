using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using TadaLib;
using TadaLib.ProcSystem;
using TadaLib.Input;

namespace TadaLib.Editor
{
    /// <summary>
    /// メモ帳起動
    /// </summary>
    public class MemoLauncher
    {
        [MenuItem("Rainier/メモ帳起動/フォルダ")]
        static void OpenMemoFolder()
        {
            System.Diagnostics.Process.Start("EXPLORER.EXE", @"Memo");
        }

        [MenuItem("Rainier/メモ帳起動/アクションメモ")]
        static void OpenMemoActionFile()
        {
            System.Diagnostics.Process.Start("notepad.exe", @"Memo/CelesteGoodPoint.txt");
        }

        [MenuItem("Rainier/メモ帳起動/エディターメモ")]
        static void OpenMemoEditorFile()
        {
            System.Diagnostics.Process.Start("notepad.exe", @"Memo/EditorSettingMemo.txt");
        }


        [MenuItem("Rainier/メモ帳起動/Todoメモ")]
        static void OpenMemoTodoFile()
        {
            System.Diagnostics.Process.Start("notepad.exe", @"Memo/TodoMemo.txt");
        }
    }
}
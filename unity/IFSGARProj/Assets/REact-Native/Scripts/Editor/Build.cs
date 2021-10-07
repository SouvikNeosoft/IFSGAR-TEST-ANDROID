using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Application;
using BuildResult = UnityEditor.Build.Reporting.BuildResult;

public class Build1 : MonoBehaviour
{
    static readonly string ProjectPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

    static readonly string apkPath = Path.Combine(ProjectPath, "Builds/" + Application.productName + ".apk");
    private static readonly string iosExportPath =
        Path.GetFullPath(Path.Combine(ProjectPath, "../../ios/UnityExport/"));

    [MenuItem("Build/Export Android %&a", false, 1)]
    public static void DoBuildAndroid()
    {
        string buildPath = Path.Combine(apkPath);
        string exportPath = Path.GetFullPath(Path.Combine(ProjectPath, "../../android/UnityExport"));

        if (Directory.Exists(apkPath))
            Directory.Delete(apkPath, true);

        if (Directory.Exists(exportPath))
            Directory.Delete(exportPath, true);

        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

        var options = BuildOptions.AllowDebugging;
        EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
        var report = BuildPipeline.BuildPlayer(
            GetEnabledScenes(),
            apkPath,
            BuildTarget.Android,
            options
        );

        if (report.summary.result != BuildResult.Succeeded)
            throw new Exception("Build failed");

        Copy(buildPath, exportPath);

        // Modify build.gradle
        var build_file = Path.Combine(exportPath, "build.gradle");
        var build_text = File.ReadAllText(build_file);
        build_text = build_text.Replace("com.android.application", "com.android.library");
        build_text = build_text.Replace("implementation fileTree(dir: 'libs', include: ['*.jar'])", "api fileTree(include: ['*.jar'], dir: 'libs')");
        // build_text = build_text.Replace("implementation(name: 'VuforiaWrapper', ext:'aar')", "api(name: 'VuforiaWrapper', ext: 'aar')");
        build_text = Regex.Replace(build_text, @"\n.*applicationId '.+'.*\n", "\n");
        File.WriteAllText(build_file, build_text);

        // Modify AndroidManifest.xml
        var manifest_file = Path.Combine(exportPath, "unityLibrary/src/main/AndroidManifest.xml"); //   -not working version, missed 'unityLibrary/' + src/main/AndroidManifest.xml");
        var manifest_text = File.ReadAllText(manifest_file);
        manifest_text = Regex.Replace(manifest_text, @"<application .*>", "<application>");
        Regex regex = new Regex(@"<activity.*>(\s|\S)+?</activity>", RegexOptions.Multiline);
        manifest_text = regex.Replace(manifest_text, "");
        File.WriteAllText(manifest_file, manifest_text);


/*

        if (Directory.Exists(apkPath))
            Directory.Delete(apkPath, true);

        if (Directory.Exists(exportPath))
            Directory.Delete(exportPath, true);

        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

        var options = BuildOptions.AcceptExternalModificationsToPlayer;
        //EditorUserBuildSettings.exportAsGoogleAndroidProject = true;

        var report = BuildPipeline.BuildPlayer(
            GetEnabledScenes(),
            apkPath,
            BuildTarget.Android,
            options
        );

        if (report.summary.result != BuildResult.Succeeded)
            throw new Exception("Build failed");
   
        Copy(buildPath, exportPath);
        //exportPath = buildPath;
        // Modify build.gradle
		var build_file = Path.Combine(exportPath, "build.gradle");
		var build_text = File.ReadAllText(build_file);
		build_text = build_text.Replace("com.android.application", "com.android.library");
        build_text = build_text.Replace("implementation fileTree(dir: 'libs', include: ['*.jar'])", "api fileTree(include: ['*.jar'], dir: 'libs')");
        // build_text = build_text.Replace("implementation(name: 'VuforiaWrapper', ext:'aar')", "api(name: 'VuforiaWrapper', ext: 'aar')");
		build_text = Regex.Replace(build_text, @"\n.*applicationId '.+'.*\n", "\n");
		File.WriteAllText(build_file, build_text);

        // Modify AndroidManifest.xml
        var manifest_file = Path.Combine(exportPath, "src/main/AndroidManifest.xml");
        var manifest_text = File.ReadAllText(manifest_file);
        manifest_text = Regex.Replace(manifest_text, @"<application .*>", "<application>");
        Regex regex = new Regex(@"<activity.*>(\s|\S)+?</activity>", RegexOptions.Multiline);
        manifest_text = regex.Replace(manifest_text, "");
        File.WriteAllText(manifest_file, manifest_text);
*/
    }

    
    [MenuItem("ReactNative/Export IOS (Unity 2019.3.*) %&i", false, 3)]
    public static void DoBuildIOS()
    {
        if (Directory.Exists(iosExportPath))
        {
            Directory.Delete(iosExportPath, true);
        }
        print("ios Path   " + iosExportPath);

        EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;

        var options = BuildOptions.AllowDebugging;
        BuildPlayerOptions options1 = new BuildPlayerOptions();
        options1.scenes = GetEnabledScenes();
        options1.locationPathName = iosExportPath;
        options1.target = BuildTarget.iOS;
        options1.options = BuildOptions.AllowDebugging ;
        var report1 = BuildPipeline.BuildPlayer(options1);
        if (report1.summary.result != BuildResult.Succeeded)
        {
            throw new Exception("Build failed");
        }
        /*
        var report = BuildPipeline.BuildPlayer(
            GetEnabledScenes(),
            iosExportPath,
            BuildTarget.iOS,
            options
        );

        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new Exception("Build failed");
        }
        */
    }

    static void Copy(string source, string destinationPath)
    {
        if (Directory.Exists(destinationPath))
            Directory.Delete(destinationPath, true);

        Directory.CreateDirectory(destinationPath);
        
        foreach (string dirPath in Directory.GetDirectories(source, "*",
            SearchOption.AllDirectories))
        {
            print(dirPath);
            Directory.CreateDirectory(dirPath.Replace(source, destinationPath));
        }
            

        foreach (string newPath in Directory.GetFiles(source, "*.*",
            SearchOption.AllDirectories))
            File.Copy(newPath, newPath.Replace(source, destinationPath), true);
    }

    static string[] GetEnabledScenes()
    {
        var scenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();

        return scenes;
    }
}

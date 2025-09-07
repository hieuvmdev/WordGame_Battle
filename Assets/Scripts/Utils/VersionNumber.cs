using UnityEngine;
using UnityEngine.UI;
using System.Reflection;


/// <summary>
/// Automatically provides a version number to a project and displays
/// it for 20 seconds at the start of the game.
/// </summary>
/// <remarks>
/// Change the first two number to update the major and minor version number.
/// The following number are the build number (which is increased automatically
///  once a day, and the revision number which is increased every second). 
/// </remarks>
[assembly:AssemblyVersion ("1.0.*")]
public class VersionNumber : MonoBehaviour
{
    /// <summary>
    /// Can be set to true, in that case the version number will be shown in bottom right of the screen
    /// </summary>
    //public bool ShowVersionInformation = false;
    /// <summary>
    /// Show the version during the first 20 seconds.
    /// </summary>
    string version;
    bool ShowVersion = false;
    public Text VersionTxt;

    /// <summary>
    /// Gets the version.
    /// </summary>
    /// <value>The version.</value>
    public string Version
    {
        get
        {
            if (version == null)
            {
                version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            return version;
        }
    }

	void Update()
	{
		
		if (Input.GetKeyDown(KeyCode.V))
		{
			ShowVersion = !ShowVersion;
		}
	    if (ShowVersion)
	    {
	        VersionTxt.text = string.Format("v{0}", Version);
        }
	    else
	    {
	        VersionTxt.text = "";
	    }
	}
}
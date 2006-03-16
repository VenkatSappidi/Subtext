#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using Subtext.Extensibility.Providers;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Configuration section handler for the <see cref="DbProvider"/>.  Most of 
	/// the implementation is in the base <see cref="ProviderSectionHandler"/> 
	/// class.  This merely follows the factory pattern in providing a method 
	/// to create a proper <see cref="ProviderConfiguration"/> instance.
	/// </summary>
	public class DbProviderSectionHandler : ProviderSectionHandler
	{
		/// <summary>
		/// Creates an <see cref="DbProviderConfiguration"/> instance which 
		/// is populated by this section handler.
		/// </summary>
		public override ProviderConfiguration CreateProviderConfigurationInstance()
		{
			return new DbProviderConfiguration();
		}
	}
}
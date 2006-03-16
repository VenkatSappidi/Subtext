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
using System.Collections;
using System.Text;

namespace Subtext.Scripting
{
	/// <summary>
	/// A collection of <see cref="Script"/>s.
	/// </summary>
	public class ScriptCollection : CollectionBase, ITemplateScript
	{
		string _fullScriptText; //Original unexpanded script.
		TemplateParameterCollection _templateParameters;

		/// <summary>
		/// Initializes a new instance of the <see cref="ScriptCollection"/> class.
		/// </summary>
		/// <param name="fullScriptText">The full script text.</param>
		public ScriptCollection(string fullScriptText)
		{
			_fullScriptText = fullScriptText;
		}

		/// <summary>
		/// Gets the <see cref="Script"/> at the specified index.
		/// </summary>
		/// <value></value>
		public Script this[int index]
		{
			get	{return ((Script)(this.List[index]));}
		}

		/// <summary>
		/// Adds the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns></returns>
		public int Add(Script value) 
		{
			return this.List.Add(value);
		}

		/// <summary>
		/// Adds the contents of another <see cref="ScriptCollection">ScriptCollection</see> 
		/// to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="ScriptCollection">ScriptCollection</see> containing the <see cref="Script"/>s to add to the collection. </param>
		public void AddRange(ScriptCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((Script)value.List[i]);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the collection contains the specified 
		/// <see cref="Script">Script</see>.
		/// </summary>
		/// <param name="value">The <see cref="Script">Script</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(Script value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional 
		/// <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> 
		/// that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(Script[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}

		/// <summary>
		/// Gets the index in the collection of the specified 
		/// <see cref="Script">Script</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="Script">Script</see> 
		/// to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(Script value) 
		{
			return this.List.IndexOf(value);
		}
		
		/// <summary>
		/// Inserts the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="value">Value.</param>
		public void Insert(int index, Script value)	
		{
			List.Insert(index, value);
		}
		
		/// <summary>
		/// Removes the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		public void Remove(Script value) 
		{
			List.Remove(value);
		}

		/// <summary>
		/// Gets the original full unexpanded script text.
		/// </summary>
		/// <value>The full script text.</value>
		public string FullScriptText
		{
			get { return _fullScriptText; }
		}

		/// <summary>
		/// Gets the expanded script text.
		/// </summary>
		/// <value>The expanded script text.</value>
		public string ExpandedScriptText
		{
			get
			{
				StringBuilder builder = new StringBuilder();
				ApplyTemplatesToScripts();
				foreach(Script script in this.List)
				{
					builder.Append(script.ScriptText);
					builder.Append(Environment.NewLine);
					builder.Append("GO");
					builder.Append(Environment.NewLine);
					builder.Append(Environment.NewLine);
				}
				return builder.ToString();
			}
		}

		internal void ApplyTemplatesToScripts()
		{
			foreach(TemplateParameter parameter in this.TemplateParameters)
			{
				foreach(Script script in this)
				{
					if(script.TemplateParameters.Contains(parameter.Name))
					{
						script.TemplateParameters[parameter.Name].Value = parameter.Value;
					}
				}
			}
		}

		/// <summary>
		/// Gets the template parameters embedded in the script.
		/// </summary>
		/// <returns></returns>
		public TemplateParameterCollection TemplateParameters
		{
			get
			{
				if(_templateParameters == null)
				{
					_templateParameters = new TemplateParameterCollection();
					foreach(Script script in this.List)
					{
						_templateParameters.AddRange(script.TemplateParameters);
					}
					_templateParameters.ValueChanged += new ParameterValueChangedEventHandler(_templateParameters_ValueChanged);
				}

				return _templateParameters;
			}
		}

		private void _templateParameters_ValueChanged(object sender, ParameterValueChangedEventArgs args)
		{
			ApplyTemplatesToScripts();
		}
	}
}
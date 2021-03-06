﻿/*
 * Creado por SharpDevelop.
 * Usuario: Supernato
 * Fecha: 06/05/2010
 * Hora: 11:27 p.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;

namespace Localizer
{
	/// <summary>
	/// Description of LanguageInformation.
	/// </summary>
	public class LanguageInformation
	{
		private String _languageName;
        private String _associatedCulture;
		private Dictionary<String, String> _strings = new Dictionary<string, string>();
		
		public LanguageInformation(String LanguageName,String AssociatedCulture, Dictionary<String, String> Strings)
		{
			_languageName = LanguageName;
			_strings = Strings;
            _associatedCulture = AssociatedCulture;
		}
		
		public String LanguageName
		{
			get { return _languageName; }
		}

        public String AssociatedCulture
        {
            get { return _associatedCulture; }
        }
		
		public Dictionary<String, String> Strings
		{
			get { return _strings; }
		}
	}
}

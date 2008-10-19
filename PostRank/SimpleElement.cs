/******************************************************************************
 * PostRank 1.0
 * Copyright 2008 Declan Whelan
 * Visit http://dpwhelan.com/postrank
 * 
 * This code is relased under the Creative Commons Attribution 3.0 License
 * http://creativecommons.org/licenses/by/3.0/
 * 
 * You are free to reuse and modify this code as long as your attribte 
 * the original author: Declan Whelan http://dpwhelan.com
 *******************************************************************************/
using System;
using System.Drawing;
using System.Globalization;
using System.Xml;

namespace PostRank
{
    public class SimpleElement
    {
        protected readonly XmlElement element;

        internal SimpleElement(XmlElement element)
        {
            if (element == null)
            {
                throw new ArgumentException();
            }

            this.element = element;
        }

        public string this[string name]
        {
            get
            {
                if (element[name] == null)
                    return null;

                return element[name].InnerText;
            }
            set
            {
                XmlElement child = element[name];

                if (value == null)
                {
                    if (child != null)
                    {
                        element.RemoveChild(child);
                    }
                }
                else
                {
                    if (child == null)
                    {
                        child = element.OwnerDocument.CreateElement(name);
                        element.AppendChild(child);
                    }
                    child.InnerText = value;
                }
            }
        }

        public string this[string child, string name]
        {
            get
            {
                if (element[child] == null)
                    return null;

                if (element[child][name] == null)
                    return null;

                return element[child][name].InnerText;
            }
        }

        internal DateTime GetDateTime(string name)
        {
            return this[name] == null ? DateTime.MinValue : DateTime.Parse(this[name]);
        }

        internal int GetLong(string name)
        {
            return this[name] == null ? 0 : int.Parse(this[name]);
        }

        internal double GetDouble(string name)
        {
            return this[name] == null ? 0 : double.Parse(this[name]);
        }

        internal Color GetColor(string name)
        {
            return this[name] == null ? Color.Empty : Color.FromArgb(int.Parse("ff" + this[name].Substring(1), NumberStyles.HexNumber));
        }

        internal Uri GetUri(string name)
        {
            return String.IsNullOrEmpty(this[name]) ? null : new Uri(this[name]);
        }

        protected void SetUri(string name, Uri value)
        {
            this[name] = value == null ? null : value.ToString();
        }

        internal Uri GetUri(string child, string name)
        {
            return String.IsNullOrEmpty(this[child, name]) ? null : new Uri(this[child, name]);
        }
    }
}
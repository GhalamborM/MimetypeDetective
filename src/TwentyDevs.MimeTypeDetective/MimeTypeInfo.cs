﻿using System;
using System.Linq;

namespace TwentyDevs.MimeTypeDetective
{
    /// <summary>
    /// Hold neccessary information about file content to deetect mime type.
    /// information like header content ,header offset ,file extention and file Mimetype string.
    /// to detection and comparing mime type these information are required.
    /// </summary>
    public class MimeTypeInfo
    {
        /// <summary>
        /// Byte array of file header.every file have a header with a certain length
        /// that use for detect type of file.
        /// </summary>
        public byte?[]      HeaderContent   { get; set; }

        /// <summary>
        /// Usually file headera start from begin of file (zero index) with no offset,
        /// but some file have a headers that start with offset like PDB,3gp,tar,mp4,...
        /// </summary>
        public int          HeaderOffset    { get; set; }

        /// <summary>
        /// File extention or a suffix at the end of a file name.
        /// start with (.) and end with  three a or four characters that are unique.
        /// </summary>
        public string       Extension       { get; private set; }

        /// <summary>
        /// Mimetype of file.a MimeType most-commonly consists of just two parts,
        /// a type and a subtype,
        /// separated by a slash (/) — with no whitespace between:
        ///  "video/3gpp","image/x-icon"
        /// </summary>
        public string       MimeType        { get; private set; }


        /// <summary>
        /// Determine this mimetype after header content must be detect by extention comaparing.
        /// some mimetype like zip and office documents have the same header content.
        /// </summary>
        public bool         MustBeDetectByExtention { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MimeTypeInfo"/> class. 
        /// </summary>
        /// <param name="headerContent">Byte array of file header</param>
        /// <param name="headerOffset">Offset of the header file</param>
        /// <param name="extension">extntion string of the file</param>
        /// <param name="mimeType"> Mimetype of file.like "video/3gpp","image/x-icon"</param>
        /// <param name="mustBeDetectByExtention">
        /// Determine this mimetype after header content must be detect by extention comaparing.
        /// some mimetype like zip and office documents have the same header content.
        /// </param>
        public MimeTypeInfo
                (
                    byte?[] headerContent, 
                    int     headerOffset, 
                    string  extension, 
                    string  mimeType,
                    bool mustBeDetectByExtention
                )
        {
            HeaderContent   = headerContent;
            HeaderOffset    = headerOffset;
            MustBeDetectByExtention = mustBeDetectByExtention;
            Extension       = NormalizeExtention(extension);
            MimeType        = NormalizeMimeType(mimeType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MimeTypeInfo"/> class.
        /// with header offset start from zero
        /// </summary>
        /// <param name="headerContent">Byte array of file header</param>
        /// <param name="extension">extntion string of the file</param>
        /// <param name="mimeType"> Mimetype of file.like "video/3gpp","image/x-icon"</param>
        /// <param name="mustBeDetectByExtention">
        /// Determine this mimetype after header content must be detect by extention comaparing.
        /// some mimetype like zip and office documents have the same header content.
        /// </param> 
        public MimeTypeInfo
                (
                    byte?[] headerContent,  
                    string  extension, 
                    string  mimeType,
                    bool mustBeDetectByExtention

                )
        {
            HeaderContent   = headerContent;
            MustBeDetectByExtention = mustBeDetectByExtention;
            Extension       = NormalizeExtention(extension);
            MimeType        = NormalizeMimeType(mimeType);
            HeaderOffset    = 0;
        }

        /// <summary>
        /// remove all space of the string and make it to lowercase.
        /// </summary>
        /// <param name="mimetypeString">
        /// determine the string of mimetype that want to be normalize.
        /// </param>
        /// <returns>return string with lowercase,with no space</returns>

        public static string NormalizeMimeType(string mimetypeString)
        {
            return RemoveSpace(mimetypeString).ToLower();
           
        }
        /// <summary>
        /// remove all space character in a string and make string to lowercase and add dot character
        /// to first of string
        /// </summary>
        /// <param name="extention"> file extention like mp3,avi, ogg , ...</param>
        /// <returns>return extention string with lowercase,dot at first and with no space</returns>
        public static string NormalizeExtention(string extention)
        {
            extention = RemoveSpace(extention).ToLower();

            if (!extention.StartsWith("."))
                extention = "." + extention;
            return extention;
        }

        /// <summary>
        /// remove all space character in the string 
        /// </summary>
        /// <param name="str">determine the string that contain space</param>
        /// <returns></returns>
        private static string RemoveSpace(string str)
        {
            while (str.Contains(" "))
                str = str.Replace(" ", "");

            return str;
        }
        /// <summary>
        /// Determines whether the specified mimetype is equal to the current mimetype by the extentions
        /// </summary>
        /// <param name="other">specified mimetype for comparing</param>
        /// <returns>true if the specified extentions extention is equal to the current mimetype extntion ,otherwise, false.</returns>
        public bool EqualsByExtention(MimeTypeInfo other)
        {
            return string.CompareOrdinal(this.Extension, other.Extension) == 0;
        }

        /// <summary>
        /// Determines whether the specified mimetype is equal to the current mimetype by the mimetype string 
        /// </summary>
        /// <param name="other">specified mimetype for comparing</param>
        /// <returns>true if the specified mimetype string is equal to the current mimetype extntion ,otherwise, false.</returns>
        public bool EqualsByMimetype(MimeTypeInfo other)
        {
            return string.CompareOrdinal(this.MimeType, other.MimeType) == 0;

        }

        /// <summary>
        /// Determines whether the specified mimetype is equal to the current mimetype by the header content 
        /// </summary>
        /// <param name="other">specified mimetype for comparing</param>
        /// <returns>true if the specified mimetype header content is equal to the current mimetype header content ,otherwise, false.</returns>
        public bool EqualsByHeaderContent(MimeTypeInfo other)
        {
            return HeaderContent.SequenceEqual(other.HeaderContent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            // not have same type
            if (!(obj is MimeTypeInfo)) 
                return false;

            var other = obj as MimeTypeInfo;


            if (other == null) 
                return false;

            if (
                    string.CompareOrdinal(this.Extension, other.Extension) == 0
                &&  string.CompareOrdinal(this.MimeType, other.MimeType) == 0
                
            )
            {
                // if both are null then they are equal
                if (this.HeaderContent == null && other.HeaderContent == null)
                    return true;

                // one of them is null then not equal
                if (this.HeaderContent == null || other.HeaderContent == null)
                    return false;

                // both have a value then compare them.
                return HeaderContent.SequenceEqual(other.HeaderContent);

            }

            return false;
        }

        public static bool operator == (MimeTypeInfo a, MimeTypeInfo b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(MimeTypeInfo a, MimeTypeInfo b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return MimeType;
        }

    }
}
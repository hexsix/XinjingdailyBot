﻿using System.Text.RegularExpressions;

namespace XinjingdailyBot.Infrastructure
{
    public static partial class RegexUtils
    {
        /// <summary>
        /// 匹配HashTag
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex("(^#\\S+)|(\\s#\\S+)")]
        public static partial Regex MatchHashTag();
        /// <summary>
        /// 匹配整个空行
        /// </summary>
        /// <returns></returns>
        [GeneratedRegex("^\\s*$")]
        public static partial Regex MatchBlankLine();
    }
}
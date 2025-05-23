﻿using FeatGen.DesktopApp.Components.Components.CodeGen;
using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Specification = FeatGen.Models.ReportGenerator.Specification;

namespace FeatGen.DesktopApp.StateContainers
{
    public class CodeGenStateContainer
    {
        private string? _reportId;
        public string ReportId
        {
            get => _reportId ?? "7a11797a-4f40-4fb5-bd43-f31a7e957df4";
            set
            {
                _reportId = value;
                NotifyStateChanged();
            }
        }

        private string? _codeRootPath;
        public string CodeRootPath
        {
            get => _codeRootPath ?? @"C:/Code/aicoding/softwarepatent";
            set
            {
                _codeRootPath = value;
                NotifyStateChanged();
            }
        }

        private ActiveFeatureBlock? _activeFeatureBlock;
        public ActiveFeatureBlock ActiveFeatureBlock
        {
            get => _activeFeatureBlock ?? new ActiveFeatureBlock();
            set
            {
                _activeFeatureBlock = value;
                NotifyStateChanged();
            }
        }

        private string _activeComponent;
        public string ActiveComponent
        {
            get => _activeComponent;
            set
            {
                _activeComponent = value;
                NotifyStateChanged();
            }
        }

        //private string _activeFeatureId;
        //public string ActiveFeatureId
        //{
        //    get => _activeFeatureId ?? string.Empty;
        //    set
        //    {
        //        _activeFeatureId = value;
        //        NotifyStateChanged();
        //    }
        //}

        //private string _activeFunctionalityId;
        //public string ActiveFunctionalityId
        //{
        //    get => _activeFunctionalityId ?? string.Empty;
        //    set
        //    {
        //        _activeFunctionalityId = value;
        //        NotifyStateChanged();
        //    }
        //}

        //private string _activeFeatureName;
        //public string ActiveFeatureName
        //{
        //    get => _activeFeatureName ?? string.Empty;
        //    set
        //    {
        //        _activeFeatureName = value;
        //        NotifyStateChanged();
        //    }
        //}

        //private string _activeFeatureCode;
        //public string ActiveFeatureCode
        //{
        //    get => _activeFeatureCode ?? string.Empty;
        //    set
        //    {
        //        _activeFeatureCode = value;
        //        NotifyStateChanged();
        //    }
        //}

        //private string _activeFunctionalityName;
        //public string ActiveFunctionalityName
        //{
        //    get => _activeFunctionalityName ?? string.Empty;
        //    set
        //    {
        //        _activeFunctionalityName = value;
        //        NotifyStateChanged();
        //    }
        //}


        //private string _activeFunctionalityCode;
        //public string ActiveFunctionalityCode
        //{
        //    get => _activeFunctionalityCode ?? string.Empty;
        //    set
        //    {
        //        _activeFunctionalityCode = value;
        //        NotifyStateChanged();
        //    }
        //}

        //private string _activeFunctionalityFilePath;
        //public string ActiveFunctionalityFilePath
        //{
        //    get => _activeFunctionalityFilePath ?? string.Empty;
        //    set
        //    {
        //        _activeFunctionalityFilePath = value;
        //        NotifyStateChanged();
        //    }
        //}

        private Specification? _specification;
        public Specification Specification
        {
            get => _specification ?? new Specification() {
                Features = new List<Feature>()
            };
            set
            {
                _specification = value;
                NotifyStateChanged();
            }
        }

        private CodeForReport? _codeForReport;
        public CodeForReport CodeForReport
        {
            get => _codeForReport ?? new CodeForReport() {
                CodeFeatures = new List<ReportCodeFeature>()
            };
            set
            {
                _codeForReport = value;
                NotifyStateChanged();
            }
        }

        private CodeTheme? _themeCode;
        public CodeTheme ThemeCode
        {
            get => _themeCode ?? new CodeTheme()
            {
                BodyBgColor = "",
                BodyBgColorDrakMode = "",
                DarkMode = "",
                FontFamily = "",
                PrimaryColor = "",
                SecondaryColor = "",
                TextColor = "",
                TextColorDarkMode = ""
            };
            set
            {
                _themeCode = value;
                NotifyStateChanged();
            }
        }


        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

    public class ActiveFeatureBlock
    {
        public string FeatureId { get; set; }
        public string FeatureName { get; set; }
        public string Code { get; set; }
        public string FunctionalityId { get; set; }
        public string FunctionalityName { get; set; }
        public string ActiveFilePath { get; set; }
    }
}

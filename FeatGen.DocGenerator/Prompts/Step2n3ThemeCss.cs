using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;

namespace FeatGen.ReportGenerator.Prompts
{
    public class Step2n3ThemeCss
    {
        public static string GlobalsCssPrompt(Specification spec)
        {
            string rawPrompt = """

                ## Context

                We're design a software named "###{service_name}###". ###{service_desc}###. For developing the software, we need to define the theme that the pages and components can follow for the software.

                We're using NextJs and React 19 to develop the software. We've already have a globals.css file that contains the theme configuration. The file is located at `app/globals.css`.

                Here's the existing css file content heritage from other software "基于光纤传感的公路基础灾害损毁感知原型系统":

                ```css       
                @import "tailwindcss";
                @import "tw-animate-css";

                /* 基于光纤传感的公路基础灾害损毁感知原型系统 */

                @custom-variant dark (&:is(.dark *));

                :root {  /* Light background for monitoring system */
                  --background-secondary: #edf2f7;  /* Secondary panel background */
                  --background-tertiary: #e2e8f0;   /* Tertiary highlights background */  /* Dark text for readability */
                  --primary: #333333;     /* Deeper fiber optic blue - represents optical transmission */
                  --secondary: #698f8e;   /* Infrastructure monitoring green - represents healthy systems */
                  --accent: #b6d10b;      /* Warning amber for alerts */
                  --sensor-gray: #4b5563; /* Neutral color for sensor hardware representations */
                  --signal-blue: #3b82f6; /* Signal transmission blue */
                  --alert-red: #ef4444;   /* Alert color for damage detection */
                  --stable-green: #22c55e; /* Status indicator for normal conditions */
                  --vibration-orange: #fb923c; /* Vibration detection color */
                  --menu-active: #000000; /* Menu active highlight - same as primary */
                  --menu-active-text: #ffffff; /* Text color for active menu items */
                  --font-family-sans: '微软雅黑', 'Microsoft YaHei', '等线', DengXian, '宋体', SimSun, sans-serif;
                  --font-family-mono: 'Consolas', 'Source Code Pro', monospace;
                  --radius: 0.625rem;
                  --background: #ffffff;
                  --foreground: #252525;
                  --card: #ffffff;
                  --card-foreground: #252525;
                  --popover: #ffffff;
                  --popover-foreground: #252525;
                  --primary-foreground: #fbfbfb;
                  --secondary-foreground: #000000;
                  --muted: #f8f8f8;
                  --muted-foreground: #8e8e8e;
                  --accent-foreground: #000000;
                  --destructive: #e03131;
                  --border: #ebebeb;
                  --input: #ebebeb;
                  --ring: #b4b4b4;
                  --chart-1: #4caf50;
                  --chart-2: #2196f3;
                  --chart-3: #3f51b5;
                  --chart-4: #ff9800;
                  --chart-5: #f44336;
                  --sidebar: #fbfbfb;
                  --sidebar-foreground: #252525;
                  --sidebar-primary: #000000;
                  --sidebar-primary-foreground: #fbfbfb;
                  --sidebar-accent: #f8f8f8;
                  --sidebar-accent-foreground: #000000;
                  --sidebar-border: #ebebeb;
                  --sidebar-ring: #b4b4b4;
                }

                @theme inline {
                  --color-background: var(--background);
                  --color-foreground: var(--foreground);
                  --font-sans: var(--font-geist-sans);
                  --font-mono: var(--font-geist-mono);
                  --color-sidebar-ring: var(--sidebar-ring);
                  --color-sidebar-border: var(--sidebar-border);
                  --color-sidebar-accent-foreground: var(--sidebar-accent-foreground);
                  --color-sidebar-accent: var(--sidebar-accent);
                  --color-sidebar-primary-foreground: var(--sidebar-primary-foreground);
                  --color-sidebar-primary: var(--sidebar-primary);
                  --color-sidebar-foreground: var(--sidebar-foreground);
                  --color-sidebar: var(--sidebar);
                  --color-chart-5: var(--chart-5);
                  --color-chart-4: var(--chart-4);
                  --color-chart-3: var(--chart-3);
                  --color-chart-2: var(--chart-2);
                  --color-chart-1: var(--chart-1);
                  --color-ring: var(--ring);
                  --color-input: var(--input);
                  --color-border: var(--border);
                  --color-destructive: var(--destructive);
                  --color-accent-foreground: var(--accent-foreground);
                  --color-accent: var(--accent);
                  --color-muted-foreground: var(--muted-foreground);
                  --color-muted: var(--muted);
                  --color-secondary-foreground: var(--secondary-foreground);
                  --color-secondary: var(--secondary);
                  --color-primary-foreground: var(--primary-foreground);
                  --color-primary: var(--primary);
                  --color-popover-foreground: var(--popover-foreground);
                  --color-popover: var(--popover);
                  --color-card-foreground: var(--card-foreground);
                  --color-card: var(--card);
                  --radius-sm: calc(var(--radius) - 4px);
                  --radius-md: calc(var(--radius) - 2px);
                  --radius-lg: var(--radius);
                  --radius-xl: calc(var(--radius) + 4px);
                }

                @media (prefers-color-scheme: dark) {
                  :root {  /* Darker monitoring panel background */
                    --background-secondary: #1e293b;  /* Secondary dark background */
                    --background-tertiary: #334155;   /* Tertiary dark background */  /* Light text for readability */
                    --primary: #2684ff;     /* Brighter fiber optic blue */
                    --secondary: #00e676;   /* Brighter infrastructure monitoring green */
                    --accent: #fbbf24;      /* Brighter amber for alerts */
                    --sensor-gray: #94a3b8; /* Lighter gray for sensor hardware */
                    --signal-blue: #60a5fa; /* Brighter signal blue */
                    --alert-red: #f87171;   /* Brighter alert red */
                    --stable-green: #86efac; /* Brighter stable green */
                    --vibration-orange: #fdba74; /* Brighter vibration orange */
                    --menu-active: #2684ff; /* Menu active highlight - same as primary */
                    --menu-active-text: #ffffff; /* Text color for active menu items */
                  }
                }

                html {
                  font-size: 16px;  /* Standard size for monitoring system readability */
                  font-family: var(--font-family-sans);
                }

                body {
                  font-family: var(--font-family-sans);
                  font-size: 1rem;
                  line-height: 1.6; /* Good line height for monitoring data */
                }

                /* Typography settings */
                h1 {
                  font-size: 2rem;    /* Appropriate size for system headers */
                  font-weight: 600;   /* Semi-bold for emphasis */
                  line-height: 1.3;   /* Adjusted line height */
                  margin-bottom: 1rem;
                  color: var(--primary);
                }

                h2 {
                  font-size: 1.75rem;
                  font-weight: 600;
                  line-height: 1.35;
                  margin-bottom: 0.8rem;
                  color: var(--foreground);
                }

                h3 {
                  font-size: 1.5rem;
                  font-weight: 500;
                  line-height: 1.4;
                  margin-bottom: 0.6rem;
                  color: var(--foreground);
                }

                p {
                  font-size: 1rem;
                  line-height: 1.6;
                  margin-bottom: 0.8rem;
                }

                /* Text size utility classes */
                .text-xs {
                  font-size: 0.75rem;    /* Smaller for technical clinical information */
                }

                .text-sm {
                  font-size: 0.875rem;   /* Better for clinical status indicators */
                }

                .text-base {
                  font-size: 1rem;       /* Standard size */
                }

                .text-lg {
                  font-size: 1.125rem;   /* Slightly larger */
                }

                .text-xl {
                  font-size: 1.25rem;    /* Adjusted size */
                }

                .text-2xl {
                  font-size: 1.5rem;     /* Adjusted size */
                }

                .text-3xl {
                  font-size: 1.75rem;    /* Adjusted size for medical content */
                }

                .text-4xl {
                  font-size: 2.25rem;    /* Adjusted size for medical headers */
                }

                /* Typography utilities */
                .font-sans {
                  font-family: var(--font-family-sans);
                }

                .font-mono {
                  font-family: var(--font-family-mono);
                }

                /* Utility classes for using the primary and secondary colors */
                .text-primary {
                  color: var(--primary);
                }

                .bg-primary {
                  background-color: var(--primary);
                }


                .text-secondary {
                  color: var(--secondary);
                }

                .bg-secondary {
                  background-color: var(--secondary);
                }

                /* Fiber optic monitoring theme specific utilities */
                .text-accent {
                  color: var(--accent);
                }

                .bg-accent {
                  background-color: var(--accent);
                }

                .text-sensor {
                  color: var(--sensor-gray);
                }

                .bg-sensor {
                  background-color: var(--sensor-gray);
                }

                .text-signal {
                  color: var(--signal-blue);
                }

                .bg-signal {
                  background-color: var(--signal-blue);
                }

                .text-alert {
                  color: var(--alert-red);
                }

                .bg-alert {
                  background-color: var(--alert-red);
                }

                .text-stable {
                  color: var(--stable-green);
                }

                .bg-stable {
                  background-color: var(--stable-green);
                }

                .text-vibration {
                  color: var(--vibration-orange);
                }

                .bg-vibration {
                  background-color: var(--vibration-orange);
                }

                /* Menu active state utilities */
                .menu-item-active {
                  color: var(--menu-active);
                  border-bottom: 2px solid var(--menu-active); /* More subtle for medical interface */
                  font-weight: 600;      /* Semi-bold for active items */
                }

                .bg-menu-active {
                  background-color: var(--menu-active);
                  color: var(--menu-active-text);
                }

                .text-menu-active {
                  color: var(--menu-active);
                }

                /* Add new utility class for combining background and text */
                .menu-active {
                  background-color: var(--menu-active);
                  color: var(--menu-active-text);
                }

                /* Clean monitoring system border style */
                .border-ink {
                  border: 1px solid var(--foreground);
                  box-shadow: 1px 1px 0 rgba(0, 0, 0, 0.1);
                }

                /* Menu styling */
                .menu-item {
                  font-size: 1.0625rem;  /* More precise for clinical interface */
                  font-weight: 500;      /* Medium weight */
                  line-height: 1.5;
                  padding: 0.5rem 0.75rem; /* Better spacing */
                  transition: all 0.2s ease;
                }

                .menu-item-sm {
                  font-size: 0.9375rem;  /* Adjusted for clinical data display */
                  font-weight: 500;
                  line-height: 1.5;
                }

                .menu-item-lg {
                  font-size: 1.125rem;   /* Adjusted for clinical interface */
                  font-weight: 600;      /* Semi-bold for emphasis */
                  line-height: 1.4;
                }

                .dropdown-menu-item {
                  font-size: 1rem;       /* Standard size for clinical dropdown items */
                  font-weight: 500;      /* Medium weight for dropdown items */
                  line-height: 1.5;
                  padding: 0.4rem 0.85rem; /* Comfortable padding */
                }

                /* Background utility classes */
                .bg-default {
                  background-color: var(--background);
                }

                .bg-alt {
                  background-color: var(--background-secondary);
                }

                .bg-highlight {
                  background-color: var(--background-tertiary);
                }

                .bg-opacity-50{
                  background-color: rgba(0, 0, 0, 0.5);
                }


                .dark {
                  --background: #252525;
                  --foreground: #fbfbfb;
                  --card: #333333;
                  --card-foreground: #fbfbfb;
                  --popover: #333333;
                  --popover-foreground: #fbfbfb;
                  --primary: #ebebeb;
                  --primary-foreground: #333333;
                  --secondary: #454545;
                  --secondary-foreground: #fbfbfb;
                  --muted: #454545;
                  --muted-foreground: #b4b4b4;
                  --accent: #454545;
                  --accent-foreground: #fbfbfb;
                  --destructive: #ff6b6b;
                  --border: rgba(255, 255, 255, 0.1);
                  --input: rgba(255, 255, 255, 0.15);
                  --ring: #8e8e8e;
                  --chart-1: #9c27b0;
                  --chart-2: #4caf50;
                  --chart-3: #f44336;
                  --chart-4: #e040fb;
                  --chart-5: #ff5722;
                  --sidebar: #333333;
                  --sidebar-foreground: #fbfbfb;
                  --sidebar-primary: #9c27b0;
                  --sidebar-primary-foreground: #fbfbfb;
                  --sidebar-accent: #454545;
                  --sidebar-accent-foreground: #fbfbfb;
                  --sidebar-border: rgba(255, 255, 255, 0.1);
                  --sidebar-ring: #8e8e8e;
                }

                @layer base {
                  * {
                    border-color: var(--border);
                  }
                  body {
                    background-color: var(--background);
                    color: var(--foreground);
                  }
                }

                /* ********************** */
                /* 基于光纤传感的公路基础灾害损毁感知原型系统 */
                /* Theme specific classes Begin */
                /* ********************** */

                .bg-signal-blue {
                  background-color: var(--signal-blue);
                }

                .bg-vibration-orange{
                  background-color: var(--vibration-orange);
                }

                .bg-alert-red{
                  background-color: var(--alert-red);
                }

                .bg-sensor-gray{
                  background-color: var(--sensor-gray);
                }

                .bg-stable-green {
                  background-color: var(--stable-green);
                }


                /* New monitoring system specific utility classes */
                .sensor-alert {
                  background-color: rgba(231, 76, 60, 0.15);
                  border-left: 3px solid var(--alert-red);
                  padding: 0.75rem;
                }

                .sensor-info {
                  background-color: rgba(75, 163, 227, 0.15);
                  border-left: 3px solid var(--signal-blue);
                  padding: 0.75rem;
                }

                .sensor-success {
                  background-color: rgba(33, 133, 89, 0.15);
                  border-left: 3px solid var(--stable-green);
                  padding: 0.75rem;
                }

                .sensor-warning {
                  background-color: rgba(255, 152, 0, 0.15);
                  border-left: 3px solid var(--vibration-orange);
                  padding: 0.75rem;
                }

                /* Card style for monitoring system interfaces */
                .sensor-card {
                  background-color: white;
                  border-radius: 0.375rem;
                  box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1), 0 1px 2px 0 rgba(0, 0, 0, 0.06);
                  padding: 1rem;
                  transition: all 0.2s ease;
                }

                .sensor-card:hover {
                  box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1), 0 2px 4px -1px rgba(0, 0, 0, 0.06);
                }

                @media (prefers-color-scheme: dark) {
                  .sensor-card {
                    background-color: var(--background-secondary);
                  }
                }

                /* Fiber optic visualization elements */
                .fiber-line {
                  height: 2px;
                  background: linear-gradient(90deg, var(--primary) 0%, var(--signal-blue) 100%);
                }

                .sensor-node {
                  width: 12px;
                  height: 12px;
                  border-radius: 50%;
                  background-color: var(--primary);
                  border: 2px solid var(--signal-blue);
                }

                .sensor-node-alert {
                  background-color: var(--alert-red);
                  border: 2px solid var(--vibration-orange);
                }

                .sensor-data-grid {
                  display: grid;
                  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
                  gap: 1rem;
                }

                /* ********************** */
                /* Theme specific classes End */
                /* ********************** */
                
                ```
                
                ## Task

                You need to update globals.css for software "###{service_name}###". You need to change the values of basic classes, variables of the software, such as colors(primary, secondary, background, text), font size, font famaliy, and so on existing in the current css file. You also need to replace the Theme specific classes of "基于光纤传感的公路基础灾害损毁感知原型系统 " to the theme of "###{service_name}###".

                Note:

                - Please keep the existing css file structure.
                - Don't remove basic tailwind css classes, only update the value
                - Replace the Theme specific classes for "###{service_name}###"
                - Don't use @apply

                ## Output format

                Return the pure code of updated globals.css file only without any explaination, markdown symboles and other characters.

                """;


            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition); 
            return prompt;
        }

    }
}

using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;

namespace FeatGen.ReportGenerator.Prompts
{
    public class GuideCodeGenThemeCssCode
    {
        public static string V1(Specification spec)
        {
            string rawPrompt = """

                ## Context

                We're design a software named "###{service_name}###". ###{service_desc}###. For developing the software, we need to define the theme that the pages and components can follow for the software.

                We're using NextJs and React 19 to develop the software. We've already have a globals.css file that contains the theme configuration. The file is located at `app/globals.css`.

                Here's the existing css file content:

                ```css       
                @tailwind base;
                @tailwind components;
                @tailwind utilities;

                @layer base {
                  :root {
                    /* Light Theme */
                    --background: 220 25% 97%;
                    --foreground: 224 71% 4%;
                    --card: 0 0% 100%;
                    --card-foreground: 224 71% 4%;
                    --popover: 0 0% 100%;
                    --popover-foreground: 224 71% 4%;
                    --primary: 240 60% 50%;
                    --primary-foreground: 0 0% 100%;
                    --secondary: 200 80% 55%;
                    --secondary-foreground: 0 0% 100%;
                    --muted: 220 14% 96%;
                    --muted-foreground: 220 10% 40%;
                    --accent: 262 83% 58%;
                    --accent-foreground: 0 0% 100%;
                    --destructive: 0 84% 60%;
                    --destructive-foreground: 210 40% 98%;
                    --border: 220 13% 91%;
                    --input: 220 13% 91%;
                    --ring: 240 60% 50%;
                    --radius: 0.5rem;

                    /* Chart Colors */
                    --chart-1: 240 60% 50%;
                    --chart-2: 200 80% 55%;
                    --chart-3: 262 83% 58%;
                    --chart-4: 330 80% 55%;
                    --chart-5: 170 75% 45%;

                    /* Brand Colors */
                    --color-1: 240 60% 50%;  /* Primary blue */
                    --color-2: 262 83% 58%;  /* Secondary purple */
                    --color-3: 200 80% 55%;  /* Accent teal */
                    --color-4: 170 75% 45%;  /* Secondary green */
                    --color-5: 330 80% 55%;  /* Accent pink */

                    --header-height: 4.5rem;
                  }

                  .dark {
                    /* Dark Theme */
                    --background: 224 25% 6%;
                    --foreground: 210 40% 98%;
                    --card: 224 25% 10%;
                    --card-foreground: 210 40% 98%;
                    --popover: 224 25% 10%;
                    --popover-foreground: 210 40% 98%;
                    --primary: 240 50% 60%;
                    --primary-foreground: 0 0% 100%;
                    --secondary: 200 70% 45%;
                    --secondary-foreground: 0 0% 100%;
                    --muted: 224 25% 16%;
                    --muted-foreground: 215 20% 65%;
                    --accent: 262 73% 63%;
                    --accent-foreground: 0 0% 100%;
                    --destructive: 0 62.8% 30.6%;
                    --destructive-foreground: 0 85.7% 97.3%;
                    --border: 224 25% 20%;
                    --input: 224 25% 20%;
                    --ring: 240 50% 60%;

                    /* Dark mode chart colors */
                    --chart-1: 240 50% 60%;
                    --chart-2: 200 70% 45%;
                    --chart-3: 262 73% 63%;
                    --chart-4: 330 70% 50%;
                    --chart-5: 170 65% 40%;
                  }
                }

                @layer base {
                  :root {
                    font-family: 'Inter', system-ui, -apple-system, BlinkMacSystemFont, sans-serif;
                    font-feature-settings: "cv02", "cv03", "cv04", "cv11", "salt";
                    font-size: 16px;
                    line-height: 1.5;
                    letter-spacing: -0.01em;
                  }

                  @supports (font-variation-settings: normal) {
                    :root {
                      font-family: 'InterVariable', system-ui, -apple-system, BlinkMacSystemFont, sans-serif;
                      font-feature-settings: "cv02", "cv03", "cv04", "cv11", "salt";
                    }
                  }

                  * {
                    @apply border-border;
                  }

                  body {
                    @apply bg-background text-foreground;
                  }

                  h1, h2, h3, h4, h5, h6 {
                    @apply font-bold tracking-tight;
                  }

                  h1 {
                    @apply text-4xl md:text-5xl;
                  }

                  h2 {
                    @apply text-3xl md:text-4xl;
                  }

                  h3 {
                    @apply text-2xl md:text-3xl;
                  }

                  /* Scrollbar styling */
                  /* width */
                  ::-webkit-scrollbar {
                    width: 6px;
                  }

                  ::-webkit-scrollbar:horizontal {
                    height: 6px;
                  }

                  /* Fix for scrollbar corner overlap */
                  ::-webkit-scrollbar-corner {
                    background: transparent;
                  }

                  ::-webkit-scrollbar-track {
                    background-color: transparent;
                  }

                  ::-webkit-scrollbar-thumb {
                    background: hsl(var(--border));
                    border-radius: 3px;
                  }

                  ::-webkit-scrollbar-thumb:hover {
                    background: hsl(var(--muted-foreground));
                  }
                }
                
                ```
                
                ## Task


                Please update globals.css for theme of software "###{service_name}###". Please update colors(primary, secondary, background, text), font size, font famaliy, and so on existing in the current css file.

                Note:

                - Please keep the existing css file structure.
                - You can add specific css styles for the software "###{service_name}###" in the file.

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

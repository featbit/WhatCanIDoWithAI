using FeatGen.Models;

namespace FeatGen.DesktopApp.PreparedCode
{
    public class IndexHtml
    {
        public static string GetHtmlContent(CodeTheme theme)
        {
            string rawPrompt = """
                <!DOCTYPE html>
                <html lang="en">
                <head>
                    <meta charset="UTF-8">
                    <meta name="viewport" content="width=device-width, initial-scale=1.0">
                    <title>Hospital Management System</title>
                    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500;700&display=swap" rel="stylesheet">
                    <script src="https://cdn.tailwindcss.com"></script>
                    <script>
                        tailwind.config = {
                            darkMode: '###{dark_mode}###',
                            theme: {
                                extend: {
                                    colors: {
                                        primary: '###{primary_color}###',
                                        secondary: '###{secondary_color}###',
                                    },
                                    fontFamily: {
                                        ###{font_family}###
                                    },
                                }
                            }
                        }
                        // Toggle dark mode
                        function toggleDarkMode() {
                            document.documentElement.classList.toggle('dark');
                        }

                        // Updated router with hash support
                        let currentPage = 'home';

                        function navigateTo(page) {
                            currentPage = page;

                            // Update URL hash without triggering another navigation
                            window.history.pushState(null, null, `#/${page}`);

                            // Update content
                            if (window.updateContent) {
                                window.updateContent(page);
                            }

                            // Update leftbar highlighting
                            if (window.highlightMenu) {
                                window.highlightMenu(page);
                            }

                            // Update topbar title
                            if (window.updatePageTitle) {
                                window.updatePageTitle(page);
                            }
                        }

                        // Handle URL hash changes
                        window.addEventListener('hashchange', function() {
                            const hash = window.location.hash.substring(2) || 'home'; // Remove #/ and default to home
                            if (hash !== currentPage) {
                                navigateTo(hash);
                            }
                        });

                        // Initialize page based on current URL hash
                        window.addEventListener('DOMContentLoaded', () => {
                            const hash = window.location.hash.substring(2) || 'home'; // Remove #/ and default to home
                            navigateTo(hash);
                        });
                    </script>
                    <style>
                        body {
                            font-family: 'Roboto', sans-serif;
                            font-size: 16px;
                            font-weight: 400;
                        }
                    </style>
                </head>

                <!-- 
                body background color in tailwind: bg-white 
                body dark mode background color in tailwind: dark:bg-gray-900
                text color in tailwind: text-gray-800
                dark mode text color in tailwind: dark:text-gray-200 
                -->
                <body class="###{body_bg_color}### ###{body_bg_color_dark_mode}### ###{text_color}### ###{text_color_dark_mode}###">
                    <div class="flex flex-col h-screen">
                        <!-- Top Bar -->
                        <div id="topbar" ></div>
                        <div class="flex flex-1 overflow-hidden">
                            <!-- Left Bar -->
                            <div id="leftbar" class="flex-shrink-0" ></div>
                            <!-- Main Content -->
                            <div id="main" class="flex-1 overflow-y-auto p-6" ></div>
                        </div>
                        <!-- Footer -->
                        <div id="footer" ></div>
                    </div>

                    <!-- Import component scripts -->
                    <script src="components/menuitems.js"></script>
                    <script src="components/topbar.js"></script>
                    <script src="components/leftbar.js"></script>
                    <script src="components/main.js"></script>
                    <script src="components/footer.js"></script>
                </body>
                </html>
                
                """;
            string prompt = rawPrompt
                .Replace("###{dark_mode}###", theme.DarkMode ?? "class")
                .Replace("###{font_family}###", theme.FontFamily ?? "'roboto': ['Roboto', 'sans-serif']")
                .Replace("###{primary_color}###", theme.PrimaryColor ?? "#8ca201")
                .Replace("###{secondary_color}###", theme.SecondaryColor ?? "#f8f8f8")
                .Replace("###{body_bg_color}###", theme.BodyBgColor ?? "bg-white")
                .Replace("###{body_bg_color_dark_mode}###", theme.BodyBgColorDrakMode ?? "dark:bg-gray-900")
                .Replace("###{text_color}###", theme.TextColor ?? "text-gray-800")
                .Replace("###{text_color_dark_mode}###", theme.TextColorDarkMode ?? "dark:text-gray-200");

            return prompt;
        }
    }
}

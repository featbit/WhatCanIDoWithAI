using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;
using FeatGen.CodingAgent.Models;
using Newtonsoft.Json;

namespace FeatGen.ReportGenerator.Prompts
{
    public class Step9Page
    {
        public static string Prompt(Specification spec, ReportCodeGuide rcg, string pageId, string menuItem, string apiCode, string cssCode, string dbCode, string dbModels, string themeIconPrompt, string themeChartPrompt)
        {
            string rawPrompt = """

                ## Context

                We're developing a software named "###{service_name}###" - ###{service_desc}###.

                We're now developing a page:
                
                ###{page_desc}###.

                We also have resources below:

                - Existing code of file "###{api_file_path_n_name}" that front-end code calls to provide the API services.
                - Existing code of database file that is used to provide the data for the API services.
                - Database models that are used for generating database file and api file.
                - File globals.css that define the css of the theme of the page.

                Frontend page code calls the APIs in "###{api_file_path_n_name}###" file to create, read, update and delete operation for buisness logic.

                ## Content and Code of Files

                - "###{api_file_path_n_name}###" file:

                ```javascript
                ###{api_code}###
                ```

                - Existing code of database file:

                ```json
                ###{db_file_code}###
                ```
                
                - Globals.css file:
              
                ```css
                ###{css_code}###
                ```

                - Database models
                
                ```javascript
                ###{db_models}###
                ```
                
                ## Task 

                Based on the requirement, specification, existing api code, database code, database models and theme css code, please generate front-end page codeby using export functions defined in api file "###{db_file_path_n_name}###".

                Note:
                
                - Front-end page should use APIs defined in the new version "###{api_file_path_n_name}###" file, `import {  } from '###{api_file_path}###';`
                - You shouldn't change existing default export function name.
                - You need to write all related components in one file instead write in other files and import it.
                - Please use className defined in the Globals.css file to style the page for background, text color, font size and family.
                - ###{theme_icon_prompt}###
                - ###{theme_chart_prompt}###
                - We're using React19, NextJs 15.2.3, Tailwind4
                - The primary language is Chinese
                - All operations on the page should be performed without redirecting or navigating to another page or route. For example, when editing an item in a table, a modal should pop up instead of navigating to a different URL.
                - The output code should be less than 1700 lines and must include the complete implementation.

                Existing front-end page code:

                ```javascript
                "use client";
                
                import { 
                } from "###{api_file_path}###";
                
                export default function ###{page_component_name}###() {  
                  return (
                    <div className="">
                    </div>
                  );
                }
                ```



                If you use toast in the page, please use existing toast components defined in the project, code like:

                ```javascript
                import { useToast } from "@/components/ui/use-toast"; // Import the internal toast hook
                import { Toaster } from "@/components/ui/toaster"; // Import the internal Toaster component
                
                // some code...
                
                const { toast } = useToast(); 
                
                // some code...

                {/* Use the internal Toaster component */}
                <Toaster />

                toast({ variant: "destructive", title: "错误", description: "未能加载用户数据。" });
                toast({ title: "成功", description: "用户资料更新成功！" });

                ```
         


                If you use `<SelectItem />` component imported from "@/components/ui/select", you must give a value. <SelectItem /> must have a value prop that is not an empty string. This is because the Select value can be set to an empty string to clear the selection and show the placeholder. For example, you should wirte the code like `<SelectItem value="*" disabled>选择一个选项</SelectItem>`, the code shouldn't be `<SelectItem value="" disabled>选择一个选项</SelectItem>`



                ## Output Format
                
                Return the pure code only without any explaination, markdown symboles and other characters.

                """;
            var menuItemsString = rcg.MenuItems;
            var menuItems = System.Text.Json.JsonSerializer.Deserialize<List<GuideMenuItem>>(menuItemsString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var pagesString = rcg.Pages;
            var allPages = System.Text.Json.JsonSerializer.Deserialize<List<GuidePageItem>>(pagesString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });
            var mainPage = allPages.FirstOrDefault(p => p.page_id == pageId);
            var subPages = allPages.Where(p =>
                    mainPage.related_pages.Any(p => p.page_id == pageId && p.direction == "forward") &&
                    menuItems.All(m => m.page_id != p.page_id)).ToList();
            var pages = new List<GuidePageItem>() { mainPage };
            pages.AddRange(subPages);
            string pageDesc = System.Text.Json.JsonSerializer.Serialize<List<GuidePageItem>>(
                            pages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            string pageComponentName = menuItem.Replace("-", "").Replace("_", "").Replace(" ", "").ToUpperInvariant();

            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{page_desc}###", pageDesc)
                .Replace("###{api_file_path_n_name}###", $"@/app/apis/{menuItem.Replace("_", "-").Trim().ToLower()}.js")
                .Replace("###{api_file_path}###", $"@/app/apis/{menuItem.Replace("_", "-").Trim().ToLower()}")
                .Replace("###{api_code}###", apiCode)
                .Replace("###{db_file_code}###", dbCode)
                .Replace("###{css_code}###", cssCode)
                .Replace("###{db_models}###", dbModels)
                .Replace("###{theme_icon_prompt}###", themeIconPrompt)
                .Replace("###{theme_chart_prompt}###", themeChartPrompt)
                .Replace("###{page_component_name}###", pageComponentName);

            return prompt;
        }

    
        public static string ToastCorrectPrompt(string existingCode)
        {
            string rawPrompt = """

                
                ## Context

                You're a specialist of nextjs development.

                If code used toast or Toaster components, and these components aren't imported from:
                
                - import { useToast } from "@/components/ui/use-toast"; // Import the internal toast hook
                - import { Toaster } from "@/components/ui/toaster"; // Import the internal Toaster component

                then you should update the code to import toasters component from "@/components/ui/use-toast" or "@/components/ui/toaster".

                Existing code:

                ```javascript
                ###{existing_code}###
                ```

                Toasts components existing in the nextjs project:

                - @/components/ui/use-toast:

                ```javascript
                import * as React from "react"

                import {
                  Toast,
                  ToastClose,
                  ToastDescription,
                  ToastProvider,
                  ToastTitle,
                  ToastViewport,
                } from "@/components/ui/toast"

                const TOAST_LIMIT = 5
                const TOAST_REMOVE_DELAY = 5000

                const actionTypes = {
                  ADD_TOAST: "ADD_TOAST",
                  UPDATE_TOAST: "UPDATE_TOAST",
                  DISMISS_TOAST: "DISMISS_TOAST",
                  REMOVE_TOAST: "REMOVE_TOAST",
                }

                let count = 0

                function genId() {
                  count = (count + 1) % Number.MAX_SAFE_INTEGER
                  return count.toString()
                }

                const toastTimeouts = new Map()

                const addToRemoveQueue = (toastId) => {
                  if (toastTimeouts.has(toastId)) {
                    return
                  }

                  const timeout = setTimeout(() => {
                    toastTimeouts.delete(toastId)
                    dispatch({
                      type: actionTypes.REMOVE_TOAST,
                      toastId,
                    })
                  }, TOAST_REMOVE_DELAY)

                  toastTimeouts.set(toastId, timeout)
                }

                export const reducer = (state, action) => {
                  switch (action.type) {
                    case actionTypes.ADD_TOAST:
                      return {
                        ...state,
                        toasts: [action.toast, ...state.toasts].slice(0, TOAST_LIMIT),
                      }

                    case actionTypes.UPDATE_TOAST:
                      return {
                        ...state,
                        toasts: state.toasts.map((t) =>
                          t.id === action.toast.id ? { ...t, ...action.toast } : t
                        ),
                      }

                    case actionTypes.DISMISS_TOAST: {
                      const { toastId } = action

                      if (toastId) {
                        addToRemoveQueue(toastId)
                      } else {
                        state.toasts.forEach((toast) => {
                          addToRemoveQueue(toast.id)
                        })
                      }

                      return {
                        ...state,
                        toasts: state.toasts.map((t) =>
                          t.id === toastId || toastId === undefined
                            ? {
                                ...t,
                                open: false,
                              }
                            : t
                        ),
                      }
                    }
                    case actionTypes.REMOVE_TOAST:
                      if (action.toastId === undefined) {
                        return {
                          ...state,
                          toasts: [],
                        }
                      }
                      return {
                        ...state,
                        toasts: state.toasts.filter((t) => t.id !== action.toastId),
                      }
                  }
                }

                const listeners = []

                let memoryState = { toasts: [] }

                function dispatch(action) {
                  memoryState = reducer(memoryState, action)
                  listeners.forEach((listener) => {
                    listener(memoryState)
                  })
                }

                function toast({ ...props }) {
                  const id = genId()

                  const update = (props) =>
                    dispatch({
                      type: actionTypes.UPDATE_TOAST,
                      toast: { ...props, id },
                    })
                  const dismiss = () => dispatch({ type: actionTypes.DISMISS_TOAST, toastId: id })

                  dispatch({
                    type: actionTypes.ADD_TOAST,
                    toast: {
                      ...props,
                      id,
                      open: true,
                      onOpenChange: (open) => {
                        if (!open) dismiss()
                      },
                    },
                  })

                  return {
                    id,
                    dismiss,
                    update,
                  }
                }

                function useToast() {
                  const [state, setState] = React.useState(memoryState)

                  React.useEffect(() => {
                    listeners.push(setState)
                    return () => {
                      const index = listeners.indexOf(setState)
                      if (index > -1) {
                        listeners.splice(index, 1)
                      }
                    }
                  }, [state])

                  return {
                    ...state,
                    toast,
                    dismiss: (toastId) => dispatch({ type: actionTypes.DISMISS_TOAST, toastId }),
                  }
                }

                function Toaster() {
                  const { toasts } = useToast()

                  return (
                    <ToastProvider>
                      {toasts.map(function ({ id, title, description, action, ...props }) {
                        return (
                          <Toast key={id} {...props}>
                            <div className="grid gap-1">
                              {title && <ToastTitle>{title}</ToastTitle>}
                              {description && (
                                <ToastDescription>{description}</ToastDescription>
                              )}
                            </div>
                            {action}
                            <ToastClose />
                          </Toast>
                        )
                      })}
                      <ToastViewport />
                    </ToastProvider>
                  )
                }

                export { useToast, toast, Toaster }
                
                ```
                
                - @/components/ui/toaster:
                
                ```javascript
                import { Toaster as ToasterImpl } from "./use-toast"

                export function Toaster() {
                  return <ToasterImpl />
                }
                
                ```
                
                ## Task 

                Please check if the existing code is using correctly the toast components, if not correct the existing code with using  "@/components/ui/use-toast" and "@/components/ui/toaster", return the new code.
                
                NOTE:
                - Keep the existing code as much as possible, only update the toast related part if needed.
                - We're using React19, NextJs 15.2.3, Tailwind4.

                ## Output Format
                
                Return the pure new code only without any explaination, markdown symboles and other characters.

                """;
            string prompt = rawPrompt.Replace("###{existing_code}###", existingCode);
            return prompt;

        }
    }


    


    
}

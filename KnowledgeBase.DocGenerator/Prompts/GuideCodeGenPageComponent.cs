using FeatGen.Models;
using FeatGen.Models.ReportGenerator;
using FeatGen.ReportGenerator.Models;
using System.Text.Json;
using FeatGen.ReportGenerator.Models.GuidePrompts;
using System.Collections.Generic;
using FeatGen.OpenAI;

namespace FeatGen.ReportGenerator.Prompts
{
    public class GuideCodeGenPageComponent
    {
        public static string V1(Specification spec, ReportCodeGuide rcg, string pageId, string pageComponentName, string apiCode, string cssCode)
        {
            string rawPrompt = """

                ## Context

                We're design a software named "###{service_name}###". ###{service_desc}###. For finishing the system, we need to design a backend api endpoints to be called by the frontend.

                In this task, we need to generate the next js page component code based on the information:

                - Page description 
                - Description of features and functionalities of the page
                - Description of inner pages or components in the page
                - APIs (or functions) that the page can use for data exchange

                ## Data and Preparation

                Here's the Page, Inner Pages/Components, Features and Functionalities:

                ###{page_features}###

                Here's API endpoints, functions and the fake data that the page can use for data exchange. This API endpoints are coded in the file `/app/api/intelligent-qa.js`:

                ```javascript
                ###{api_endpoints}###
                ```

                Here's the global css classname that you can use

                ```css
                ###{css_code}###
                ```


                ## Task

                Based on the information above, please regenerate the code for this nextjs page file located at 'src/app/intelligent-qa/page.js'.

                Note:

                - You shouldn't change existing default export function name.
                - You can add more code or functions if needed.

                ## Output format

                Return the pure code only without any explaination, markdown symboles and other characters.


                """;
            var menuItemsString = rcg.MenuItems.CleanJsCodeQuote().CleanJsonCodeQuote();
            var menuItems = JsonSerializer.Deserialize<List<GuideMenuItem>>(menuItemsString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var pagesString = rcg.Pages.CleanJsCodeQuote().CleanJsonCodeQuote();
            var allPages = JsonSerializer.Deserialize<List<GuidePageItem>>(pagesString, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            var mainPage = allPages.FirstOrDefault(p => p.page_id == pageId);
            var subPages = allPages.Where(p =>
                    p.related_pages.Any(p => p.page_id == pageId) &&
                    menuItems.All(m => m.page_id != p.page_id)).ToList();

            var pages = new List<GuidePageItem>() { mainPage };
            pages.AddRange(subPages);

            string pageDesc = JsonSerializer.Serialize<List<GuidePageItem>>(
                            pages, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });

            cssCode = """
                /* Text size utility classes */
                .text-xs 
                
                .text-sm 
                
                .text-base
                
                .text-lg
                
                .text-xl
                
                .text-2xl
                
                .text-3xl
                
                .text-4xl
                
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
                
                /* Guilin theme specific utilities */
                .text-accent {
                  color: var(--accent);
                }
                
                .bg-accent {
                  background-color: var(--accent);
                }
                
                .text-mountain {
                  color: var(--mountain);
                }
                
                .bg-mountain {
                  background-color: var(--mountain);
                }
                
                .text-water {
                  color: var(--water);
                }
                
                .bg-water {
                  background-color: var(--water);
                }
                
                /* Decorative Chinese ink brush style border */
                .border-ink {
                  border: 2px solid var(--foreground);
                  box-shadow: 2px 2px 0 rgba(0, 0, 0, 0.1);
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
                ```
                
                Page to be generated is located at folder "/app/intelligent-qa/page.js" . Existing code in the NextJs page file:
                
                ```javascript
                export default function ###{page_component_name}###() {  
                  return (
                    <div className="py-6 flex h-[calc(100vh-64px)]">
                      {/* Left sidebar - Sessions history */}
                    </div>
                  );
                }
                """;

            apiCode = """
                import { v4 as uuidv4 } from 'uuid';

                // In-memory storage for our models
                const memoryDB = {
                  qa_sessions: [
                    {
                      session_id: "qa-session-1",
                      user_id: "user-1",
                      title: "如何使用Python进行数据分析",
                      created_at: "2023-04-15T09:30:00Z",
                      updated_at: "2023-04-15T10:45:00Z",
                      is_archived: false
                    },
                    {
                      session_id: "qa-session-2",
                      user_id: "user-2",
                      title: "机器学习模型评估指标",
                      created_at: "2023-04-16T14:20:00Z",
                      updated_at: "2023-04-16T15:35:00Z",
                      is_archived: false
                    }
                  ],
                  qa_messages: [
                    {
                      message_id: "msg-1",
                      session_id: "qa-session-1",
                      sender_type: "user",
                      content: "Python如何进行数据分析？有哪些常用库？",
                      created_at: "2023-04-15T09:30:00Z",
                      is_read: true
                    },
                    {
                      message_id: "msg-2",
                      session_id: "qa-session-1",
                      sender_type: "ai",
                      content: "Python数据分析常用的库包括Pandas、NumPy、Matplotlib和Seaborn等。Pandas提供数据结构和数据分析工具，NumPy支持大型多维数组和矩阵运算，Matplotlib和Seaborn则用于数据可视化。基本分析流程包括数据导入、清洗、探索性分析、可视化和建模。",
                      created_at: "2023-04-15T09:31:00Z",
                      is_read: true
                    }
                  ],
                  feedback: [
                    {
                      feedback_id: "fb-1",
                      message_id: "msg-2",
                      user_id: "user-1",
                      rating: 5,
                      comment: "回答非常详细，提供了所有我需要的库",
                      created_at: "2023-04-15T09:35:00Z"
                    }
                  ],
                  knowledge_areas: [
                    {
                      area_id: "ka-1",
                      name: "Python编程",
                      description: "Python相关的编程知识和技巧",
                      created_at: "2023-03-01T00:00:00Z"
                    },
                    {
                      area_id: "ka-2",
                      name: "机器学习",
                      description: "机器学习算法、模型和应用",
                      created_at: "2023-03-01T00:00:00Z"
                    }
                  ],
                  suggested_questions: [
                    {
                      question_id: "sq-1",
                      session_id: "qa-session-1",
                      content: "如何使用Pandas处理缺失数据？",
                      is_clicked: false,
                      created_at: "2023-04-15T09:32:00Z"
                    },
                    {
                      question_id: "sq-2",
                      session_id: "qa-session-1",
                      content: "Matplotlib和Seaborn有什么区别？",
                      is_clicked: false,
                      created_at: "2023-04-15T09:32:00Z"
                    }
                  ]
                };

                // QA Session Operations
                export const createQASession = async (userId, title = "新对话") => {
                  const now = new Date().toISOString();
                  const newSession = {
                    session_id: uuidv4(),
                    user_id: userId,
                    title,
                    created_at: now,
                    updated_at: now,
                    is_archived: false
                  };

                  memoryDB.qa_sessions.push(newSession);
                  return newSession;
                };

                export const getQASessionById = async (sessionId) => {
                  return memoryDB.qa_sessions.find(session => session.session_id === sessionId) || null;
                };

                export const getUserQASessions = async (userId) => {
                  return memoryDB.qa_sessions
                    .filter(session => session.user_id === userId)
                    .sort((a, b) => new Date(b.updated_at) - new Date(a.updated_at));
                };

                export const updateQASessionTitle = async (sessionId, newTitle) => {
                  const session = memoryDB.qa_sessions.find(session => session.session_id === sessionId);
                  if (!session) {
                    throw new Error('Session not found');
                  }

                  session.title = newTitle;
                  session.updated_at = new Date().toISOString();
                  return session;
                };

                export const archiveQASession = async (sessionId) => {
                  const session = memoryDB.qa_sessions.find(session => session.session_id === sessionId);
                  if (!session) {
                    throw new Error('Session not found');
                  }

                  session.is_archived = true;
                  session.updated_at = new Date().toISOString();
                  return session;
                };

                // QA Message Operations
                export const sendUserMessage = async (sessionId, userId, content) => {
                  // Validate session exists
                  const session = memoryDB.qa_sessions.find(s => s.session_id === sessionId);
                  if (!session) {
                    throw new Error('Session not found');
                  }

                  const now = new Date().toISOString();

                  // Create user message
                  const userMessage = {
                    message_id: uuidv4(),
                    session_id: sessionId,
                    sender_type: 'user',
                    content,
                    created_at: now,
                    is_read: true
                  };

                  memoryDB.qa_messages.push(userMessage);

                  // Update session time
                  session.updated_at = now;

                  return userMessage;
                };

                export const getAIResponse = async (sessionId, userMessageId) => {
                  // Get user message to provide context for AI response
                  const userMessage = memoryDB.qa_messages.find(m => m.message_id === userMessageId);
                  if (!userMessage) {
                    throw new Error('User message not found');
                  }

                  // Generate AI response (in a real system, this would call an AI service)
                  // Here we'll simulate with a mock response
                  const now = new Date().toISOString();
                  const responseContent = generateMockAIResponse(userMessage.content);

                  const aiMessage = {
                    message_id: uuidv4(),
                    session_id: sessionId,
                    sender_type: 'ai',
                    content: responseContent,
                    created_at: now,
                    is_read: false
                  };

                  memoryDB.qa_messages.push(aiMessage);

                  // Update session last activity
                  const session = memoryDB.qa_sessions.find(s => s.session_id === sessionId);
                  if (session) {
                    session.updated_at = now;
                  }

                  // Generate suggested follow-up questions
                  generateSuggestedQuestions(sessionId, userMessage.content);

                  return aiMessage;
                };

                export const getSessionMessages = async (sessionId) => {
                  return memoryDB.qa_messages
                    .filter(message => message.session_id === sessionId)
                    .sort((a, b) => new Date(a.created_at) - new Date(b.created_at));
                };

                export const markMessageAsRead = async (messageId) => {
                  const message = memoryDB.qa_messages.find(m => m.message_id === messageId);
                  if (!message) {
                    throw new Error('Message not found');
                  }

                  message.is_read = true;
                  return message;
                };

                // Feedback Operations
                export const submitFeedback = async (messageId, userId, rating, comment = '') => {
                  // Check if message exists
                  const message = memoryDB.qa_messages.find(m => m.message_id === messageId);
                  if (!message) {
                    throw new Error('Message not found');
                  }

                  // Check if message is from AI (only AI messages can receive feedback)
                  if (message.sender_type !== 'ai') {
                    throw new Error('Feedback can only be given for AI responses');
                  }

                  // Create feedback entry
                  const feedback = {
                    feedback_id: uuidv4(),
                    message_id: messageId,
                    user_id: userId,
                    rating,
                    comment,
                    created_at: new Date().toISOString()
                  };

                  memoryDB.feedback.push(feedback);
                  return feedback;
                };

                export const getFeedbackForMessage = async (messageId) => {
                  return memoryDB.feedback.find(fb => fb.message_id === messageId) || null;
                };

                export const getFeedbackStatistics = async (userId) => {
                  const userFeedback = memoryDB.feedback.filter(fb => fb.user_id === userId);

                  const totalCount = userFeedback.length;
                  let positiveCount = 0;
                  let neutralCount = 0;
                  let negativeCount = 0;

                  userFeedback.forEach(fb => {
                    if (fb.rating >= 4) positiveCount++;
                    else if (fb.rating >= 2) neutralCount++;
                    else negativeCount++;
                  });

                  return {
                    totalCount,
                    positiveCount,
                    neutralCount,
                    negativeCount,
                    positivePercentage: totalCount > 0 ? Math.round((positiveCount / totalCount) * 100) : 0
                  };
                };

                // Suggested Questions Operations
                export const getSuggestedQuestions = async (sessionId) => {
                  return memoryDB.suggested_questions
                    .filter(question => question.session_id === sessionId && !question.is_clicked)
                    .sort((a, b) => new Date(b.created_at) - new Date(a.created_at));
                };

                export const markSuggestedQuestionAsClicked = async (questionId) => {
                  const question = memoryDB.suggested_questions.find(q => q.question_id === questionId);
                  if (!question) {
                    throw new Error('Suggested question not found');
                  }

                  question.is_clicked = true;
                  return question;
                };

                // Knowledge Area Operations
                export const getKnowledgeAreas = async () => {
                  return memoryDB.knowledge_areas;
                };

                export const getKnowledgeAreaById = async (areaId) => {
                  return memoryDB.knowledge_areas.find(area => area.area_id === areaId) || null;
                };
                """;

            string prompt = rawPrompt
                .Replace("###{service_name}###", spec.Title)
                .Replace("###{service_desc}###", spec.Definition)
                .Replace("###{page_features}###", pageDesc)
                .Replace("###{api_endpoints}###", apiCode)
                .Replace("###{css_code}###", cssCode)
                .Replace("###{page_component_name}###", pageComponentName);
            return prompt;
        }
    }


    
}

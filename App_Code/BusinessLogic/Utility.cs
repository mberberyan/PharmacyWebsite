using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessLogic
{
    /// <summary>
    /// Summary description for Utility
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Finds a sitemap node with specified attribute "code".
        /// </summary>
        /// <param name="nodeCode">the value of the attribute "code".</param>
        /// <returns>SiteMapNode object</returns>
        public static SiteMapNode FindNodeByCode(string nodeCode)
        {
            SiteMapNode resultNode = FindChildNodebyCode(SiteMap.RootNode, nodeCode);
            return resultNode;
        }

        /// <summary>
        /// Finds a sitemap node with specified attribute "code" and parent the passed parent SitemapNode.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="nodeCode"></param>
        /// <returns></returns>
        private static SiteMapNode FindChildNodebyCode(SiteMapNode parentNode, string nodeCode)
        {
            SiteMapNode resultNode = null;
            if (parentNode.HasChildNodes)
            {
                foreach (SiteMapNode node in parentNode.ChildNodes)
                {
                    if (node["code"] == nodeCode)
                    {
                        resultNode = node;
                        break;
                    }
                    if (node.HasChildNodes)
                    {
                        resultNode = FindChildNodebyCode(node, nodeCode);
                        if (resultNode != null)
                        {
                            break;
                        }
                    }

                }
            }

            return resultNode;
        }

        /// <summary>
        /// Returns url of the parent from second level in the sitemap of specified site map node.
        /// </summary>
        /// <param name="node">Site map node.</param>
        /// <returns>Url of parent sitemap node fom second level in the sitemap heirarchy.</returns>
        public static string GetFirstLevelParentUrl(SiteMapNode node)
        {
            SiteMapNode currentNode = node;
            if (currentNode.ParentNode != null && currentNode.ParentNode["code"] != null)
            {
                while (currentNode.ParentNode.ParentNode != null)
                {
                    currentNode = currentNode.ParentNode;
                }
            }
            return currentNode.Url;
        }

        /// <summary>
        /// Returns code of the parent from second level in the sitemap of specified site map node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static string GetFirstLevelParentCode(SiteMapNode node)
        {
            SiteMapNode currentNode = node;
            if (currentNode.ParentNode != null && currentNode.ParentNode["code"] != null)
            {
                while (currentNode.ParentNode.ParentNode != null)
                {
                    currentNode = currentNode.ParentNode;
                }
            }
            return currentNode["code"];
        }
    }
}

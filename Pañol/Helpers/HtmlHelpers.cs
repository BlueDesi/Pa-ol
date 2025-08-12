using System.Web.Mvc;

namespace Pañol.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString BootstrapCreateButton(this HtmlHelper htmlHelper, string actionName, string controllerName, string buttonText = "Agregar Nuevo")
        {
            var url = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            string href = url.Action(actionName, controllerName);

            string buttonHtml = $@"
                <a href='{href}' class='btn btn-success btn-lg'>
                    <i class='bi bi-plus-circle me-2'></i> {buttonText}
                </a>";

            return new MvcHtmlString(buttonHtml);
        }

        public static MvcHtmlString BootstrapEditButton(this HtmlHelper htmlHelper, string actionName, string controllerName, object routeValues, string buttonText = "Editar")
        {
            var url = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            string href = url.Action(actionName, controllerName, routeValues);

            string buttonHtml = $@"
                <a href='{href}' class='btn btn-warning btn-lg text-white'>
                    <i class='bi bi-pencil-square me-2'></i> {buttonText}
                </a>";

            return new MvcHtmlString(buttonHtml);
        }

        public static MvcHtmlString BootstrapDeleteButton(this HtmlHelper htmlHelper, string actionName, string controllerName, object routeValues, string buttonText = "Eliminar")
        {
            var url = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            string href = url.Action(actionName, controllerName, routeValues);

            string buttonHtml = $@"
                <a href='{href}' class='btn btn-danger btn-lg' onclick='return confirm(""¿Estás seguro que querés eliminar?"");'>
                    <i class='bi bi-trash me-2'></i> {buttonText}
                </a>";

            return new MvcHtmlString(buttonHtml);
        }

        public static MvcHtmlString BootstrapBackButton(this HtmlHelper htmlHelper, string actionName, string controllerName, string buttonText = "Volver")
        {
            var url = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            string href = url.Action(actionName, controllerName);

            string buttonHtml = $@"
                <a href='{href}' class='btn btn-secondary btn-lg'>
                    <i class='bi bi-arrow-left me-2'></i> {buttonText}
                </a>";

            return new MvcHtmlString(buttonHtml);
        }
    }
}

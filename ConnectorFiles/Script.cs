public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
        if (this.Context.OperationId == "stringtouppercase")
        {
            return await this.HandleToUpperCase().ConfigureAwait(false);
        }
        else if (this.Context.OperationId == "sortstringarray")
        {
            return await this.HandleSort().ConfigureAwait(false);
        }
        else if (this.Context.OperationId == "jsonecho")
        {
            return await this.HandleEcho().ConfigureAwait(false);
        }

        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent($"Unknown operation ID '{this.Context.OperationId}'");
        return response;
    }

    private async Task<HttpResponseMessage> HandleEcho()
    {
        HttpResponseMessage response;
        var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = new StringContent(contentAsString);
        return response;
    }
    
    private async Task<HttpResponseMessage> HandleToUpperCase()
    {
        HttpResponseMessage response;
        var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = new StringContent(contentAsString?.ToUpper());
        return response;
    }

    private async Task<HttpResponseMessage> HandleSort()
    {
        HttpResponseMessage response;
        var contentAsString = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        var stringJArray = JArray.Parse(contentAsString);
        var stringArray = stringJArray.ToObject<List<string>>();
        stringArray.Sort();
        var result = JArray.FromObject(stringArray);

        response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = CreateJsonContent( result.ToString());
        return response;
    }
}

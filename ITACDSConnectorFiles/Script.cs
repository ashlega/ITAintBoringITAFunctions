public class Script : ScriptBase
{
    public override async Task<HttpResponseMessage> ExecuteAsync()
    {
	if (this.Context.OperationId == "userinfo")
        {
            return await this.GetUserInfo().ConfigureAwait(false);
        }

        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        response.Content = CreateJsonContent($"Unknown operation ID '{this.Context.OperationId}'");
        return response;
    }

    private async Task<HttpResponseMessage> GetUserInfo()
    {
        // Use the context to forward/send an HTTP request
        var userName = await this.Context.Request.Content.ReadAsStringAsync().ConfigureAwait(false);
        this.Context.Request.RequestUri = new Uri($"https://orgd27b211f.api.crm.dynamics.com/api/data/v9.2/systemusers?$filter=contains(fullname,{userName})");
        this.Context.Request.Method = HttpMethod.Get;
        HttpResponseMessage response = await this.Context.SendAsync(this.Context.Request, this.CancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        return response;
    }
}

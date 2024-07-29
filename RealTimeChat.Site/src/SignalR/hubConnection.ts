import * as signalR from "@microsoft/signalr";
import { baseUrl } from "../api/axiosConfig";

const chatHubBaseUrl = `${baseUrl}/chathub`

const hubConnection = new signalR.HubConnectionBuilder()
  .withUrl(chatHubBaseUrl)
  .configureLogging(signalR.LogLevel.Information)
  .build();

export default hubConnection;

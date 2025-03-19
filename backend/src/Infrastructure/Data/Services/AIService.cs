using GenerativeAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Infrastructure.Data.Services;

public class AIService : IAIService
{
    private readonly ILogger<AIService> _logger;
    private readonly IConfiguration _configuration;

    public AIService(ILogger<AIService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    public async Task<Priority> AnalyzeTaskPriorityAsync(string description)
    {
        try
        {
            var apiKey = _configuration.GetValue<string>("AIService:ApiKey");

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("API key for AI service is not configured.");
            }

            var model = new GenerativeModel(apiKey, "gemini-2.0-flash"); // Use gemini-2.0-flash

            var prompt = $@"
            Given the following task description, suggest a priority level (High, Medium, Low).

            Task Description: {description}

            Priority Level:
            ";

            var response = await model.GenerateContentAsync(prompt);
            string? prioritySuggestion = response.Text();
            _logger.LogInformation("AI response: {PrioritySuggestion}", prioritySuggestion);

            // Clean up the response (remove extra text, whitespace)
            prioritySuggestion = prioritySuggestion?.Trim();

            if (prioritySuggestion != null && prioritySuggestion.Contains("**", StringComparison.CurrentCultureIgnoreCase))
            {
                prioritySuggestion = ExtractTextBetweenDelimiters(prioritySuggestion, "**", "**");
            }

            return prioritySuggestion switch
            {
                "High" => Priority.High,
                "Medium" => Priority.Medium,
                "Low" => Priority.Low,
                _ => Priority.None
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing task priority with AI service");
            // Default to None priority if AI analysis fails
            return Priority.None;
        }
    }

    private static string ExtractTextBetweenDelimiters(string input, string startDelimiter, string endDelimiter)
    {
        // Find the starting position of the first delimiter
        int startIndex = input.IndexOf(startDelimiter);
        if (startIndex == -1)
            return string.Empty;

        // Adjust startIndex to the end of the first delimiter
        startIndex += startDelimiter.Length;

        // Find the ending position (starting from where the first delimiter ends)
        int endIndex = input.IndexOf(endDelimiter, startIndex);
        if (endIndex == -1)
            return string.Empty;

        // Extract and return the text between the delimiters
        return input.Substring(startIndex, endIndex - startIndex);
    }
}

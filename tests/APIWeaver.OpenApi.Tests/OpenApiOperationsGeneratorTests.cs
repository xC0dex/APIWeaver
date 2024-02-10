using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

namespace APIWeaver.OpenApi.Tests;

public class OpenApiOperationsGeneratorTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    [Fact]
    public async Task GenerateOperationsAsync_ShouldShould_WhenWhen()
    {
        // Arrange
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services => services.AddApiWeaver(options => options.GeneratorOptions.OperationTransformers.Add(context => context.OpenApiOperation.Summary = "Awesome endpoint")));
        }).CreateClient();

        // Act
        var response = await client.GetAsync("/swagger/v1-openapi.json");
        response.EnsureSuccessStatusCode();


        var content = await response.Content.ReadAsStringAsync();

        // Assert
        var expected = $$"""
                         {
                           "openapi": "3.0.1",
                           "info": {
                             "title": "APIWeaver.OpenApi.MinimalApi",
                             "version": "1.0.0"
                           },
                           "servers": [
                             {
                               "url": "{{client.BaseAddress!.AbsoluteUri.TrimEnd('/')}}"
                             }
                           ],
                           "paths": {
                             "/user": {
                               "get": {
                                 "tags": [
                                   "APIWeaver.OpenApi.MinimalApi"
                                 ],
                                 "summary": "Awesome endpoint",
                                 "responses": {
                                   "200": {
                                     "description": "OK",
                                     "content": {
                                       "application/json": {
                                         "schema": {
                                           "uniqueItems": false,
                                           "type": "array",
                                           "items": {
                                             "$ref": "#/components/schemas/User"
                                           }
                                         }
                                       }
                                     }
                                   }
                                 }
                               },
                               "post": {
                                 "tags": [
                                   "APIWeaver.OpenApi.MinimalApi"
                                 ],
                                 "summary": "Awesome endpoint",
                                 "requestBody": {
                                   "content": {
                                     "application/json": {
                                       "schema": {
                                         "$ref": "#/components/schemas/User"
                                       }
                                     }
                                   },
                                   "required": true
                                 },
                                 "responses": {
                                   "200": {
                                     "description": "OK",
                                     "content": {
                                       "application/json": {
                                         "schema": {
                                           "$ref": "#/components/schemas/User"
                                         }
                                       }
                                     }
                                   }
                                 }
                               }
                             },
                             "/user/{id}": {
                               "get": {
                                 "tags": [
                                   "APIWeaver.OpenApi.MinimalApi"
                                 ],
                                 "summary": "Awesome endpoint",
                                 "parameters": [
                                   {
                                     "name": "id",
                                     "in": "path",
                                     "required": true,
                                     "schema": {
                                       "type": "string",
                                       "format": "uuid"
                                     }
                                   }
                                 ],
                                 "responses": {
                                   "200": {
                                     "description": "OK",
                                     "content": {
                                       "application/json": {
                                         "schema": {
                                           "$ref": "#/components/schemas/User"
                                         }
                                       }
                                     }
                                   }
                                 }
                               }
                             }
                           },
                           "components": {
                             "schemas": {
                               "User": {
                                 "required": [
                                   "id",
                                   "name"
                                 ],
                                 "type": "object",
                                 "properties": {
                                   "id": {
                                     "type": "string",
                                     "format": "uuid"
                                   },
                                   "name": {
                                     "type": "string"
                                   },
                                   "age": {
                                     "type": "integer",
                                     "format": "int32",
                                     "nullable": true
                                   },
                                   "books": {
                                     "uniqueItems": false,
                                     "type": "array",
                                     "items": {
                                       "$ref": "#/components/schemas/Book"
                                     },
                                     "nullable": true
                                   }
                                 },
                                 "additionalProperties": false
                               },
                               "Book": {
                                 "required": [
                                   "id",
                                   "name"
                                 ],
                                 "type": "object",
                                 "properties": {
                                   "id": {
                                     "type": "string"
                                   },
                                   "name": {
                                     "type": "string"
                                   }
                                 },
                                 "additionalProperties": false
                               }
                             }
                           }
                         }
                         """;
        content.ReplaceLineEndings().Should().Be(expected);
    }
}
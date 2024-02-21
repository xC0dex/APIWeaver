using Microsoft.AspNetCore.Mvc.Testing;

namespace APIWeaver.OpenApi.Tests;

public class ControllerOpenApiOperationGeneratorTests(WebApplicationFactory<ControllerApiProgram> factory): IClassFixture<WebApplicationFactory<ControllerApiProgram>>
{
    [Fact]
    public async Task GenerateOperationsAsync_ShouldShould_WhenWhen()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger/v1-openapi.json");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        
        // Assert
        var expected = $$"""
                         {
                           "openapi": "3.0.1",
                           "info": {
                             "title": "APIWeaver.OpenApi.ControllerApi",
                             "version": "1.0.0"
                           },
                           "servers": [
                             {
                               "url": "{{client.BaseAddress!.AbsoluteUri.TrimEnd('/')}}"
                             }
                           ],
                           "paths": {
                             "/User": {
                               "get": {
                                 "tags": [
                                   "User"
                                 ],
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
                                   "500": {
                                     "description": "Internal Server Error",
                                     "content": {
                                       "text/plain": {
                                         "schema": {
                                           "$ref": "#/components/schemas/User"
                                         }
                                       },
                                       "application/json": {
                                         "schema": {
                                           "$ref": "#/components/schemas/User"
                                         }
                                       },
                                       "text/json": {
                                         "schema": {
                                           "$ref": "#/components/schemas/User"
                                         }
                                       }
                                     }
                                   },
                                   "200": {
                                     "description": "OK",
                                     "content": {
                                       "text/plain": {
                                         "schema": {
                                           "$ref": "#/components/schemas/User"
                                         }
                                       },
                                       "application/json": {
                                         "schema": {
                                           "$ref": "#/components/schemas/User"
                                         }
                                       },
                                       "text/json": {
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

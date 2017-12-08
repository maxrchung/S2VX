#version 330 core
out vec4 FragColor;
  
in vec2 TexCoord;

uniform sampler2D texture1;
uniform sampler2D texture2;

void main()
{
    vec2 flipCoord = vec2(1 - TexCoord.x, TexCoord.y);
    FragColor = mix(texture(texture1, flipCoord), texture(texture2, flipCoord), 0.2);
}
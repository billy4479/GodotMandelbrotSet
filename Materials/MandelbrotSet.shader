shader_type canvas_item;

uniform int MAX_ITERATIONS = 100;
uniform vec2 minCoords;
uniform vec2 maxCoords;
uniform vec2 screenSize;

vec2 getComplexCoords(vec2 screenCoords) {
	return vec2(
		(screenCoords.x * (maxCoords.x - minCoords.x) / screenSize.x) + minCoords.x,
		(screenCoords.y * (maxCoords.y - minCoords.y) / screenSize.y) + minCoords.y
	);
}

int getIterationCount(vec2 coords){
	int iterations = 0;
	float x = 0.0;
	float y = 0.0;
	
	while (x*x + y*y <= 4.0 && iterations < MAX_ITERATIONS){
		float xTemp = (x*x - y*y) + coords.x;
		y = 2.0*x*y + coords.y;
		x = xTemp;
		iterations++;
	}
	return iterations;
}

void fragment(){
	vec4 screenCoords = FRAGCOORD;
	vec2 complexCoords = getComplexCoords(vec2(floor(screenCoords.x), floor(screenCoords.y)));
	int iterations = getIterationCount(complexCoords);

	if (iterations == MAX_ITERATIONS){
		COLOR = vec4(0,0,0,1);
		return;
	}

//	uint c = uint(round(float(MAX_ITERATIONS)*sqrt(float(iterations)/float(MAX_ITERATIONS))));
//	float v = float(c)/255.0;
//	COLOR = vec4(v, v, v, 1);

	float hue = abs(360f * float(iterations) / float(MAX_ITERATIONS) - 360f);
	float x = 1f - abs(mod((hue/60f),2f)-1f);
	
	if(hue < 60f) COLOR = vec4(1, x, 0, 1);
	else if (hue < 120f) COLOR = vec4(x, 1, 0, 1);
	else if (hue < 180f) COLOR = vec4(0, 1, x, 1);
	else if (hue < 240f) COLOR = vec4(0, x, 1, 1);
	else if (hue < 300f) COLOR = vec4(x, 0, 1, 1);
	else COLOR = vec4(1, 0, x, 1);
}
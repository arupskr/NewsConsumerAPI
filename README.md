# NewsConsumerAPI
The solution is still not complete but some assumptioms and remarks have been made below to improve the design and code and align more to the 
requirement

- Assumptions n Practices:
1. Caching of the response for 5 mins have been implemented in the Story Controller assuming that the content and its attributes do not change often
2. Swagger Open API is integrated to the core api to provide an interactive documentation as the API grows. Started with V1
3. The caching could be moved to external Redis in a more real life use case. But Redis is not great in storing large strings as key value pairs.
The best stories will have to be stored by their individual id 
4. If the number of stories to be returned increases to 50 or more, consider batching the parallel fetch. It helps in exception handling
5. Top 20 as Take(20) as been used to extract the first 20 ids to fetch the stories
6. There is an integration test for the stories controller which asserts the count of the stories return and validates the response code. The coverage at this stage is not good enough due to time constraint.

- TODO:
1. The JSON attributes still needs to be displayed as per the specification
2. I have just noticed that the sorting by score was missed, but it can be easily implemented
.OrderByDescending(x => x.Score);
3. the Time to UTC time

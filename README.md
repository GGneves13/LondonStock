# LondonStock
Welcome to my implementation of LondonStock
Please note that when the Web API is started, no stocks are available. Please add some stocks of your choosing. 

The solution has 2 controllers, one intended just for reading, another intended for writing on the DB, to split the workload. One day these could be pointing to different DBs.
LondonStockRead has two endpoints:
[Get]	/api/LondonStockRead/GetAllStocks
	No params
[Post]	/api/LondonStockRead/GetStocks 
	Params: List<string>

LondonStockUpdate has two endpoitns:
[Post]	/api/LondonStockUpdate/AddNewStock
	Params: stockSymbol 	(string) 	- Must not be empty
			value 			(decimal)  	- Must be > 0
[Post]	/api/LondonStockUpdate/AddNewOrder
	Params: stockSymbol 	(string) 	- Must not be empty
			price 			(decimal) 	- Must be > 0
			numberOfShares 	(decimal) 	- Must be > 0
			brokerId 		(int) 		- Must be > 0

All endpoints should be more secure and a user/password pair should be added for all brokers.
Whitelisting of IPs should also be added to the solution to provide better protection from unwanted requests.

Due to the time constraint, the unit tests do not cover the entire solution, but there is a structure which I believe is enough to understand where the development was going.


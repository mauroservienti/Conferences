Conferences samples repository

Each conference has its own branch

---

# Disclaimer
These samples, as most samples, are focused on a specific problem, or a set of, in an oversimplified way.

All the code in these samples is not intended to be in any way production code and is not intended to be a definitive solution, nor the best neither the correct one, to a problem.

All these samples are under constant evolution and are all thought to be used during talks at conferences or at least during courses, this is the main reason of their oversimplification.

## Urls

* Create Person:
    * http://localhost:1454/api/jason/createNewPerson
    * method":"POST"
    * "headers":"Content-Type: application/json",
    * "data":"{ firstName: '', lastName: '' }"

* PersonView / 1:
    * http://localhost:1454/api/People/1"
    * "method":"GET"
    * "headers":"Content-Type: application/json"

* CompanyView / 1:
    * http://localhost:1454/api/Companies/1
    * "method":"GET"
    * "headers":"Content-Type: application/json"

* Get PersonViews
    * http://localhost:1454/api/People
    * "method":"GET"
    * "headers":"Content-Type: application/json"

* Search w/ Suggestions
    * http://localhost:1454/api/Search?q=tpics.i
    * "method":"GET"
    * "headers":"Content-Type: application/json"

* Create company
    * http://localhost:1454/api/jason/createNewCompany
    * "method":"POST"
    * "headers":"Content-Type: application/json"
    * "data":"{ companyName: '', vatNumber: '' }"

* Get CompanyViews
    * http://localhost:1454/api/Companies
    * "method":"GET"
    * "headers":"Content-Type: application/json"

* Search m*
    * http://localhost:1454/api/Search?q=m*
    * "method":"GET"
    * "headers":"Content-Type: application/json"
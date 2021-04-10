Implementation of PointOfSaleTerminal with tests.

Domain ideas for future PointOfSaleTerminal extending/improvement for cashier:
* to add/remove/update payment rules
* to print receipt with correct order of products and other customizations
* to scan X items in a row (f.e. eggs)
* to remove already scanned product or set the price explicitly (for market diector f.e.)
* to work with different currencies
* to set some special discount for some clients/for some products
* etc.

Infrastructure ideas for improving existing code:
* add analyzers to fail build until all warnings are fixed
* add persistence layers
* add presentation layers
* move application to .Net6 with long term support
* try to use Functional programming features where needed
* write integration tests when application grows
* setup GitHub stuff like CI/Wiki/merging strategy/policies/etc.
* etc.
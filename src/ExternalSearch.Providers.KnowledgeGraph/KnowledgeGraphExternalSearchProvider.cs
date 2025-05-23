﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KnowledgeGraphExternalSearchProvider.cs" company="Clued In">
//   Copyright Clued In
// </copyright>
// <summary>
//   Defines the KnowledgeGraphExternalSearchProvider type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Crawling.Helpers;
using CluedIn.ExternalSearch.Filters;
using CluedIn.ExternalSearch.Providers.KnowledgeGraph.Model;
using CluedIn.ExternalSearch.Providers.KnowledgeGraph.Vocabularies;
using RestSharp;
using System.Web;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.ExternalSearch;
using CluedIn.Core.Providers;
using EntityType = CluedIn.Core.Data.EntityType;
using CluedIn.Core.Data.Vocabularies;
using CluedIn.Core.Connectors;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.KnowledgeGraph
{
    /// <summary>The knowledge graph external search provider.</summary>
    /// <seealso cref="CluedIn.ExternalSearch.IExternalSearchResultLogger" />
    /// <seealso cref="CluedIn.ExternalSearch.ExternalSearchProviderBase" />
    public partial class KnowledgeGraphExternalSearchProvider : ExternalSearchProviderBase, IExternalSearchResultLogger, IExtendedEnricherMetadata, IConfigurableExternalSearchProvider, IExternalSearchProviderWithVerifyConnection
    {
        /**********************************************************************************************************
         * FIELDS
         **********************************************************************************************************/

        private static readonly EntityType[] DefaultAcceptedEntityTypes = { EntityType.Organization };

        /**********************************************************************************************************
         * FIELDS
         **********************************************************************************************************/

        /// <summary>The type names</summary>
        private readonly HashSet<string> typeNames = new[]
            {
                "3D printing company",
                "3d printing company",
                "AI company",
                "Access control company",
                "Accessories retailer company",
                "Accounting company",
                "Active wear company",
                "Ad company",
                "Adhesive manufacturing company",
                "Adoption company",
                "Advertising agency company",
                "Advertising company",
                "Aeronautics company",
                "Aerospace and defense company",
                "Aerospace company",
                "Aerospace engineering company",
                "Affiliate marketing company",
                "Aftermarket company",
                "Agricultural machinery company",
                "Agriculture company",
                "Agrochemical company",
                "Air courier services company",
                "Aircraft company",
                "Aircraft engineering company",
                "Aircraft industry company",
                "Aircraft manufacturer",
                "Aircraft manufacturing company",
                "Airline",
                "Airport",
                "Alcoholic beverage company",
                "All-inclusive resort company",
                "Alternative energy company",
                "Alternative investment company",
                "Aluminium company",
                "Aluminum company",
                "Ambulance services company",
                "Amusement Park",
                "Amusement and theme parks company",
                "Amusement park company",
                "Analytical laboratory instrument manufacturing company",
                "Analytics company",
                "Animation company",
                "Apparel company",
                "Application service provider company",
                "Aquaculture company",
                "Arcade game company",
                "Architectural services company",
                "Architecture company",
                "Arms industry company",
                "Arms manufacturers company",
                "Artificial intelligence company",
                "Arts company",
                "Asset management company",
                "Assisted living company",
                "Assisted living facility company",
                "Auction company",
                "Audio mastering company",
                "Auto parts company",
                "Auto racing company",
                "Auto repair company",
                "Automaker company",
                "Automatic vending machine manufacturing company",
                "Automation company",
                "Automobile company",
                "Automobile racing company",
                "Automotive company",
                "Automotive design company",
                "Automotive engineering company",
                "Automotive industry company",
                "Automotive repair and maintenance company",
                "Automotive web company",
                "Aviation company",
                "Backpacking company",
                "Bakery company",
                "Baking company",
                "Ballet company",
                "Bank",
                "Bank holding company",
                "Banking company",
                "Bar company",
                "Basement waterproofing company",
                "Bathroom company",
                "Bed company",
                "Bedding company",
                "Belt company",
                "Beverage company",
                "Beverage production company",
                "Beverages company",
                "Bicycle company",
                "Bicycle industry company",
                "Bicycle manufacture company",
                "Bicycle-sharing system company",
                "Bicycling company",
                "Big data company",
                "Big-box store company",
                "Biometrics company",
                "Biopharmaceutical company",
                "Biotech company",
                "Biotechnology company",
                "Bitcoin company",
                "Board game company",
                "Boating company",
                "Bond credit rating company",
                "Book publishers company",
                "Bookstore company",
                "Boutique investment banking company",
                "Brain training company",
                "Brand company",
                "Branding agency company",
                "Breweries company",
                "Brewery",
                "Brewery company",
                "Brewing company",
                "British mower company",
                "Broadband company",
                "Broadcast syndication company",
                "Broadcasting company",
                "Broadcasting television network",
                "Brokerage firm company",
                "Browser game company",
                "Buffet company",
                "Building automation company",
                "Building construction company",
                "Building material company",
                "Bus company",
                "Bus manufacturing company",
                "Business aviation company",
                "Business incubator company",
                "Business intelligence company",
                "Business intelligence software company",
                "Business networking company",
                "Business process automation company",
                "Business process management company",
                "Business process outsourcing company",
                "Business reputation management company",
                "Business services company",
                "Business software company",
                "CAD company",
                "CGI company",
                "CPU company",
                "CRM company",
                "Cable channel",
                "Cable company",
                "Cable television company",
                "Cable tv company",
                "Cafe company",
                "Camping company",
                "Capital Region of",
                "Capital management company",
                "Car company",
                "Car dealership company",
                "Car insurance company",
                "Car rental agency company",
                "Car rental company",
                "Car tuning company",
                "Caravan company",
                "Cargo company",
                "Carpet cleaning company",
                "Carsharing company",
                "Cashback website company",
                "Casino company",
                "Catering company",
                "Cellular operator company",
                "Cellular phone provider company",
                "Cement company",
                "Cement manufacturing company",
                "Central bank company",
                "Ceramic company",
                "Certification company",
                "Chain store company",
                "Charitable organization",
                "Charitable organization company",
                "Charity company",
                "Charter airline company",
                "Cheese manufacturing company",
                "Chemical industry company",
                "Chemical manufacturing company",
                "Chemicals company",
                "Chief Executive Officer of the Acushnet Company",
                "Chimney sweep company",
                "Chocolate company",
                "Chronometer company",
                "Cinema company",
                "Civil engineering company",
                "Classified advertising company",
                "Classifieds company",
                "Clean technology company",
                "Cloth company",
                "Clothing company",
                "Clothing retail company",
                "Clothing retailer company",
                "Clothing stores company",
                "Cloud computing company",
                "Cloud computing security company",
                "Cloud storage company",
                "Coaching company",
                "Coal company",
                "Coal mining company",
                "Coffee company",
                "Coffee production company",
                "Coffee shops company",
                "Coffeehouse company",
                "Cogeneration company",
                "Collaboration company",
                "Collaborative software company",
                "Collectable company",
                "College",
                "College in ",
                "Colocation company",
                "Comics company",
                "Commercial banking company",
                "Commercial company",
                "Commercial organization",
                "Commercial property company",
                "Commodity company",
                "Communication company",
                "Communications equipment company",
                "Community-based organization",
                "Company",
                "Company type",
                "Compliance company",
                "Computer and video games company",
                "Computer animation company",
                "Computer company",
                "Computer data storage company",
                "Computer display company",
                "Computer game company",
                "Computer hardware company",
                "Computer industry company",
                "Computer manufacturing company",
                "Computer network company",
                "Computer networking company",
                "Computer programming company",
                "Computer programs company",
                "Computer science company",
                "Computer security company",
                "Computer software company",
                "Computer-aided engineering company",
                "Computer-generated imagery company",
                "Computing company",
                "Concept store company",
                "Concrete company",
                "Confectionery company",
                "Conglomerate company",
                "Consignment company",
                "Console game company",
                "Construction company",
                "Construction machinery and equipment company",
                "Construction management company",
                "Construction of buildings company",
                "Consultant company",
                "Consulting company",
                "Consumer banking company",
                "Consumer electronics company",
                "Consumer finance company",
                "Content management company",
                "Content management system company",
                "Content marketing company",
                "Convenience store company",
                "Convenience stores company",
                "Conversion rate optimization company",
                "Cooling solutions company",
                "Copper mining company",
                "Corporate group company",
                "Corporate investment banking company",
                "Cosmetics company",
                "Coupon company",
                "Courier company",
                "Coverage company",
                "Crawl space repair company",
                "Credit bureau company",
                "Credit card company",
                "Credit card service company",
                "Credit company",
                "Credit management company",
                "Credit rating agency company",
                "Credit union company",
                "Cremation company",
                "Crowdfunding company",
                "Crowdsourcing company",
                "Customer experience company",
                "Customer relationship management company",
                "Customer service company",
                "Cybersecurity company",
                "Cycling company",
                "Daily newspaper company",
                "Dairy company",
                "Dairy product manufacturing company",
                "Data center company",
                "Data entry services company",
                "Data recovery company",
                "Data visualization company",
                "Database company",
                "Debt collection company",
                "Defense contractor company",
                "Delivery company",
                "Dental equipment and supplies manufacturing company",
                "Dental hygiene company",
                "Dentist company",
                "Dentistry company",
                "Department store company",
                "Design company",
                "Diamond company",
                "Digital currency exchange company",
                "Digital distribution company",
                "Digital marketing company",
                "Digital media company",
                "Digital signage company",
                "Digital television company",
                "Dime store company",
                "Diner company",
                "Direct marketing company",
                "Disaster recovery company",
                "Discount store company",
                "Distilleries company",
                "Distribution company",
                "Diversified corporation company",
                "Document imaging company",
                "Dress company",
                "Drilling oil and gas wells company",
                "Drink company",
                "Drop shipping company",
                "Dynamic DNS company",
                "E-commerce company",
                "E-discovery company",
                "E-mail marketing company",
                "ECommerce company",
                "EMS company",
                "Eatery company",
                "Education company",
                "Educational services company",
                "Educational software company",
                "Educational technology company",
                "Electric bicycle company",
                "Electric car company",
                "Electric lighting equipment manufacturing company",
                "Electric power distribution company",
                "Electric power transmission company",
                "Electric services company",
                "Electric utility company in Geertruidenberg, Netherlands",
                "Electric utility company in Klaipeda, Lithuania",
                "Electric utility company in Lübeck, Germany",
                "Electric utility company in Odense, Denmark",
                "Electric utility company in Poland",
                "Electric vehicle company",
                "Electrical engineering company",
                "Electrical equipment manufacturing company",
                "Electricity company",
                "Electricity corporation",
                "Electricity generation company",
                "Electronic commerce company",
                "Electronic component manufacturing company",
                "Electronic connector manufacturing company",
                "Electronic design automation company",
                "Electronic game company",
                "Electronics company",
                "Electronics industry company",
                "Electronics manufacturing company",
                "Electronics manufacturing services company",
                "Electronics retail company",
                "Electronics retailer company",
                "Email marketing company",
                "Embroidery company",
                "Emergency medical services company",
                "Employment agency company",
                "Energy company",
                "Energy industry company",
                "Energy storage company",
                "Engineering and construction services company",
                "Engineering and design company",
                "Engineering company",
                "Engineering services company",
                "Engineering, procurement and construction company",
                "English company",
                "Enterprise content management company",
                "Enterprise information management company",
                "Enterprise software company",
                "Entertainment company",
                "Envelope manufacturing company",
                "Environmental consulting company",
                "Equity crowdfunding company",
                "Equity investment company",
                "Estate agent company",
                "Event management company",
                "Executive Chairman of the Hudson's Bay Company",
                "Executive search company",
                "Eyelash extensions company",
                "FX company",
                "Fabless manufacturing company",
                "Fabric company",
                "Fabric mills company",
                "Facilities management company",
                "Facility management company",
                "Fair trade company",
                "Family office company",
                "Farming company",
                "Fashion accessory company",
                "Fashion boutique company",
                "Fashion company",
                "Fashion design company",
                "Fast casual restaurant company",
                "Fast food company",
                "Fast food restaurant company",
                "Fast-moving consumer goods company",
                "Feminine hygiene company",
                "Ferry company",
                "Film company",
                "Film distribution company",
                "Film industry company",
                "Film production company",
                "Film studio company",
                "Filtration company",
                "Finance and insurance company",
                "Finance company",
                "Finance service company",
                "Financial accounting company",
                "Financial management company",
                "Financial market company",
                "Financial planning company",
                "Financial services company",
                "Financial technology company",
                "Financing company",
                "Firearm company",
                "Firefighting company",
                "Fishing company",
                "Fitness center company",
                "Fleet management company",
                "Floor covering company",
                "Flooring company",
                "Food and beverage company",
                "Food company",
                "Food conglomerate company",
                "Food distribution company",
                "Food industry company",
                "Food manufacturing company",
                "Food processing company",
                "Food safety company",
                "Food truck company",
                "Foodservice company",
                "Football Organization",
                "Footwear company",
                "Footwear manufacturing company",
                "Footwear retail company",
                "Footwear retailer company",
                "Foreign exchange company",
                "Forestry company",
                "Forex company",
                "Forging company",
                "Foundry company",
                "Franchising company",
                "Free software company",
                "Freelance marketplace company",
                "Freight forwarder company",
                "Fuel cell company",
                "Fuel industry company",
                "Full service investment banking company",
                "Fund management company",
                "Funding company",
                "Fundraising company",
                "Fur trade company",
                "Furniture company",
                "Furniture retail company",
                "Furniture retailer company",
                "Furniture stores company",
                "GIS company",
                "GPS company",
                "Gambling company",
                "Garden company",
                "Gardening company",
                "Garment company",
                "Gas industry company",
                "General aviation company",
                "General contractor company",
                "General insurance company",
                "General store company",
                "Geographic information system company",
                "Glass art company",
                "Glass company",
                "Glassware company",
                "Gold company",
                "Gold mining company",
                "Golf Facility",
                "Golf company",
                "Golf course company",
                "Golf equipment company",
                "Graphic cards company",
                "Graphic design company",
                "Greeting cards company",
                "Grocery store company",
                "Guarantee company",
                "Gun company",
                "Guns company",
                "Gym company",
                "HVAC company",
                "Hair care company",
                "Hard drive disk company",
                "Hardware company",
                "Hardware store company",
                "Health care company",
                "Health company",
                "Health food company",
                "Health insurance company",
                "Healthcare company",
                "Hedge fund company",
                "High street store company",
                "High-intensity focused ultrasound company",
                "Higher education company",
                "Holding company",
                "Home appliance company",
                "Home automation company",
                "Home care company",
                "Home construction company",
                "Home furnishings company",
                "Home improvement company",
                "Home loan company",
                "Home products retailer company",
                "Home video company",
                "Hong Kong company",
                "Hospice company",
                "Hospital",
                "Hospital company",
                "Hospitality company",
                "Hospitals company",
                "Hot tub company",
                "Hotel",
                "Hotel company",
                "Household appliances company",
                "Human resources company",
                "Humanitarian aid company",
                "Humanitarian aid organization",
                "Hydraulics company",
                "Hydroponics company",
                "Hygiene company",
                "Hypermarket company",
                "ICT company",
                "IP communications company",
                "IP telephony company",
                "IT company",
                "IT security company",
                "IT service management company",
                "IT services company",
                "ITIL company",
                "Ice cream company",
                "Ice cream parlor company",
                "Ice hockey equipment company",
                "Ice manufacturing company",
                "Identity management company",
                "Illustration company",
                "Inbound marketing company",
                "Independent school",
                "Indian company",
                "Industrial conglomerate company",
                "Industrial design company",
                "Industrial engineering company",
                "Industrial gases company",
                "Industry company",
                "Information and communications technology company",
                "Information management company",
                "Information security company",
                "Information technology company",
                "Information technology consulting company",
                "Infrastructure company",
                "Instrumentation company",
                "Insurance carriers company",
                "Insurance company",
                "Insurance service company",
                "Integrated marketing communications company",
                "Intellectual property company",
                "Interactive entertainment company",
                "Interactive media company",
                "International conglomerate company",
                "Internet advertising company",
                "Internet company",
                "Internet marketing company",
                "Internet of things company",
                "Internet radio company",
                "Internet security company",
                "Internet service provider company",
                "Internet services company",
                "Internet telephony company",
                "Investment advice company",
                "Investment banking company",
                "Investment company",
                "Investment management company",
                "Investment service company",
                "Iron ore company",
                "Islamic banking and finance company",
                "Islamic banking company",
                "Italian food company",
                "Jeans company",
                "Jewellery company",
                "Jewelry company",
                "Jewelry design company",
                "Jewelry stores company",
                "Jewelry, precious metal company",
                "Kitchen company",
                "Knowledge management system company",
                "Land company",
                "Law company",
                "Law firm company",
                "Leasing company",
                "Legal notary public company",
                "Legal services company",
                "Legal technology company",
                "Leisure company",
                "Letting agent company",
                "Life insurance company",
                "Lighting company",
                "Lightness company",
                "Lingerie company",
                "Lingerie retailer company",
                "Local newspaper company",
                "Local search company",
                "Logistics company",
                "Lottery company",
                "Lumber company",
                "Luxury goods company",
                "Luxury vehicles company",
                "Machine company",
                "Machine industry company",
                "Machine tool company",
                "Mail company",
                "Mail order company",
                "Major appliance manufacturing company",
                "Mall company",
                "Managed care company",
                "Management company",
                "Management consulting company",
                "Management consulting services (marketing consulting) company",
                "Management consulting services company",
                "Management training company",
                "Manufacturing company",
                "Marine insurance company",
                "Marine services company",
                "Market research company",
                "Marketing automation company",
                "Marketing communications company",
                "Marketing company",
                "Marketing strategy company",
                "Marketplace company",
                "Mass media company",
                "Material company",
                "Material handling company",
                "Meat packer company",
                "Meat packing plants company",
                "Mechanical engineering company",
                "Mechatronics company",
                "Media company",
                "Media conglomerate company",
                "Medical device company",
                "Medical equipment company",
                "Medical marijuana company",
                "Medicine company",
                "Medium duty truck company",
                "Memory company",
                "Menswear company",
                "Merchandising company",
                "Metallurgy company",
                "Metalworking company",
                "Microelectronics company",
                "Microfinance company",
                "Microfinancing company",
                "Mining company",
                "Mining company in Finland",
                "Mining company in the Democratic Republic of the Congo",
                "Mobile advertising company",
                "Mobile app company",
                "Mobile application development company",
                "Mobile banking company",
                "Mobile commerce company",
                "Mobile device management company",
                "Mobile digital communication company",
                "Mobile game company",
                "Mobile marketing company",
                "Mobile network operator company",
                "Mobile network provider company",
                "Mobile operator company",
                "Mobile payment company",
                "Mobile phone network company",
                "Mobile phone operator company",
                "Mobile security company",
                "Mobile software company",
                "Mobile telecommunication company",
                "Modeling agency company",
                "Modem company",
                "Money management company",
                "Mortgage loan company",
                "Motherboard company",
                "Motion picture company",
                "Motion pictures/entertainment company",
                "Motor sports company",
                "Motorcycle company",
                "Motorsport company",
                "Mountaineering company",
                "Movie company",
                "Movie theater company",
                "Moving company",
                "Multi-industry company company",
                "Multi-level marketing company",
                "Multimedia company",
                "Multinational conglomerate company",
                "Municipality",
                "Museum",
                "Music company",
                "Music creating company",
                "Music education company",
                "Music industry company",
                "Music label",
                "Music learning company",
                "Music publisher company",
                "Music publishing company",
                "Music recording company",
                "Musical instrument company",
                "Musical instrument manufacturing company",
                "Mutual fund company",
                "Nanotechnology company",
                "National commercial banks company",
                "National newspaper company",
                "Natural gas company",
                "Natural gas distribution company",
                "Natural gas transmission company",
                "Natural language processing company",
                "Network marketing company",
                "Network security company",
                "Networking hardware company",
                "News agency company",
                "News company company",
                "News media company",
                "News service company",
                "Newspaper company",
                "Newspaper publishers company",
                "Nightclub company",
                "Non profit corporation",
                "Non profit organization",
                "Non profit trade organization",
                "Non-governmental organization",
                "Non-profit organization",
                "Non-profit public corporation",
                "Non-profit-making organization",
                "Nonprofit organization",
                "Not for profit corporation",
                "Not-for-profit organization",
                "Nuclear electric power generation company",
                "Nuclear energy company",
                "Nuclear power company",
                "Nutrition company",
                "Office supplies company",
                "Offshore drilling company",
                "Oil industry company",
                "Oilfield services company",
                "Online advertising company",
                "Online auction company",
                "Online bingo company",
                "Online casino company",
                "Online dating service company",
                "Online food ordering company",
                "Online gambling company",
                "Online game company",
                "Online marketing company",
                "Online marketplace company",
                "Online music store company",
                "Online pharmacy company",
                "Online poker company",
                "Online retail company",
                "Online retailer company",
                "Online service provider company",
                "Online shopping company",
                "Online store company",
                "Online travel agency company",
                "Open-source software company",
                "Opera Company",
                "Optics company",
                "Optoelectronics company",
                "Organic food company",
                "Outerwear company",
                "Outlet store company",
                "Outsourcing company",
                "PC case company",
                "PC game company",
                "Packaging and labeling company",
                "Paint and coating manufacturing company",
                "Paper company",
                "Paper mill company",
                "Paperboard mills company",
                "Parking company",
                "Passenger rail transport company",
                "Payment service provider company",
                "Payment system company",
                "Peer-to-peer lending company",
                "Pen company",
                "Pension fund company",
                "Performance management company",
                "Personal care company",
                "Personal computer company",
                "Personal computer equipment company",
                "Personal development company",
                "Personal finance company",
                "Personal financial management company",
                "Personal injury company",
                "Pest control company",
                "Pet food company",
                "Pet insurance company",
                "Pet supply company",
                "Petroleum industry company",
                "Petroleum refineries company",
                "Pharmaceutical company",
                "Pharmaceutical industry company",
                "Pharmaceutics company",
                "Pharmacy company",
                "Photography company",
                "Photonics company",
                "Photovoltaic power company",
                "Photovoltaics company",
                "Physical fitness company",
                "Pillow company",
                "Pipeline transportation of crude oil company",
                "Pizzeria company",
                "Plastic company",
                "Plastics material and resin manufacturing company",
                "Platform as a service company",
                "Plumbing company",
                "Podcast company",
                "Pop-up retail company",
                "Pornographic film company",
                "Port wine company",
                "Post-production company",
                "Postal service company",
                "Pottery company",
                "Power engineering company",
                "Printing company",
                "Private banking company",
                "Private company",
                "Private equity company",
                "Private investment banking company",
                "Private investment management company",
                "Private jet company",
                "Private school",
                "Private school company",
                "Private university",
                "Privately held company",
                "Process engineering company",
                "Product design company",
                "Production company",
                "Professional and management development training company",
                "Professional sports company",
                "Professional wrestling company",
                "Programming company",
                "Project management company",
                "Property developer company",
                "Property insurance company",
                "Property management company",
                "Pub chain company",
                "Pub company",
                "Public company",
                "Public finance company",
                "Public relations company",
                "Public transport company",
                "Public utility company",
                "Publication company",
                "Publishing company",
                "Pulp and paper industry company",
                "Radio and television broadcasting company",
                "Radio broadcasting stations company",
                "Radio network company",
                "Radio networks company",
                "Radio-frequency identification company",
                "Rail company",
                "Rail transit company",
                "Rail transport company",
                "Railroad company in Zürich, Switzerland",
                "Railroad transportation company",
                "Railway company",
                "Railway station",
                "Railway transportation company",
                "Real estate company",
                "Real estate development company",
                "Real estate investment company",
                "Real estate investment management company",
                "Real estate investment trust company",
                "Realty company",
                "Recommender system company",
                "Recommerce company",
                "Record company",
                "Record label",
                "Record label company",
                "Record store company",
                "Recording studio company",
                "Recreational vehicle company",
                "Recruitment company",
                "Recycling company",
                "Regulation company",
                "Reinsurance company",
                "Religious broadcasting company",
                "Relocation company",
                "Remittance company",
                "Renewable energy company",
                "Research and development company",
                "Research and development tax company",
                "Research company",
                "Resort company",
                "Restaurant",
                "Restaurant company",
                "Retail banking company",
                "Retail chain company",
                "Retail company",
                "Retail forex company",
                "Retail outlet company",
                "Retail-store company",
                "Risk management company",
                "Road bicycle company",
                "Robotics company",
                "Rocket engine company",
                "Role-playing game company",
                "Roller coaster company",
                "SEO company",
                "SaaS company",
                "Salons company",
                "Satellite company",
                "Satellite television company",
                "Savings bank company",
                "School",
                "School in",
                "Science company",
                "Science research company",
                "Scuba diving company",
                "Search engine company",
                "Search engine marketing company",
                "Search engine optimization company",
                "Search marketing company",
                "Securities brokerage company",
                "Security company",
                "Security printing company",
                "Security systems company",
                "Seed company",
                "Self storage company",
                "Semantic web company",
                "Semiconductor company",
                "Semiconductor device fabrication company",
                "Semiconductor industry company",
                "Semiconductor manufacturing company",
                "Service design company",
                "Service industries company",
                "Sewing company",
                "Sex industry company",
                "Ship management company",
                "Shipbuilding company",
                "Shipping company",
                "Shipping line company",
                "Shoe company",
                "Shoe manufacturing company",
                "Shoe retail company",
                "Shoe retailer company",
                "Shoe stores company",
                "Shopping centre",
                "Shopping mall",
                "Shopping mall company",
                "Skateboarding company",
                "Ski resort",
                "Skiing company",
                "Skilled nursing care facilities company",
                "Small arms manufacturing company",
                "Smart card company",
                "Social commerce company",
                "Social media company",
                "Social network company",
                "Social software company",
                "Software as a service company",
                "Software company",
                "Software development company",
                "Software engineering company",
                "Software industry company",
                "Software testing company",
                "Solar energy company",
                "Solar energy company in India",
                "Solar power company",
                "Solid-state drive company",
                "Sound cards company",
                "Sound effect company",
                "Spa company",
                "Space industry company",
                "Space technology company",
                "Space tourism company",
                "Spacecraft company",
                "Spanish company",
                "Spanish satellite broadcasting company",
                "Specialty retail company",
                "Specialty retailer company",
                "Sporting goods stores company",
                "Sports company",
                "Sports equipment company",
                "Sports organization",
                "Sportswear company",
                "Stadium",
                "Stainless steel production company",
                "Standards organization",
                "Startup company",
                "State commercial banks company",
                "Stationery company",
                "Steel company",
                "Steel industry company",
                "Steel production company",
                "Steelmaking company",
                "Stem cell research company",
                "Stock broker company",
                "Stock exchange company",
                "Stock investment company",
                "Stock photography company",
                "Streaming media company",
                "Structural engineering company",
                "Structured finance company",
                "Sugar beet company",
                "Suit company",
                "Summer camp company",
                "Supermarket company",
                "Superstore company",
                "Supply chain company",
                "Supply chain management company",
                "Supply chain solutions company",
                "Surveying company",
                "Sustainable tourism company",
                "Swiss company",
                "Swiss security company",
                "System integration company",
                "TV Network",
                "TV company",
                "Tableware company",
                "Tavern company",
                "Tea company",
                "Teaching company",
                "Tech company",
                "Technology company",
                "Technology transfer company",
                "Telecom company",
                "Telecommunications company",
                "Telecommunications equipment company",
                "Telecoms equipment company",
                "Telematics company",
                "Telemetry company",
                "Telephone service company",
                "Television broadcasting company",
                "Television company",
                "Television network",
                "Television production company",
                "Television station",
                "Terminating deposit company",
                "Test preparation company",
                "Testing laboratories company",
                "Textile company",
                "Textile industry company",
                "Textile manufacturing company",
                "Theater company",
                "Theatre company",
                "Theme Park",
                "Theme park company",
                "Thrift store company",
                "Ticket company",
                "Timber company",
                "Timepiece company",
                "Tire company",
                "Tire manufacturing company",
                "Title insurance company",
                "Title loan company",
                "Tobacco company",
                "Tour operator company",
                "Tourism company",
                "Toy company",
                "Trade company",
                "Trade credit insurance company",
                "Train operating company",
                "Training company",
                "Translation company",
                "Transport company",
                "Transport operator",
                "Travel agencies company",
                "Travel agency company",
                "Travel insurance company",
                "Travel website company",
                "Tree climbing company",
                "Truck company",
                "Tutoring company",
                "Tyre company",
                "Underwear company",
                "Unified communications company",
                "University",
                "Uranium mining company",
                "Urban planning company",
                "Utilities company",
                "Utility company",
                "VFX company",
                "Valuation company",
                "Variety shop company",
                "Variety store company",
                "Vending company",
                "Ventilation company",
                "Venture capital company",
                "Venture capital financing company",
                "Venture investment company",
                "Vice Chairman of the Hudson's Bay Company",
                "Video Game Developer",
                "Video game company",
                "Video game developer company",
                "Video game development company",
                "Video game industry company",
                "Video game publisher company",
                "Video on demand company",
                "Video production company",
                "Video rental shop company",
                "Video sharing company",
                "Video surveillance company",
                "Video tape and disc rental company",
                "Videogame company",
                "Virtual office company",
                "Vision technologies company",
                "Visual effects company",
                "Visual effects company company",
                "VoIP company",
                "Voice over IP company",
                "Voluntary organization",
                "Wallpaper company",
                "Warehouse store company",
                "Warranty company",
                "Waste company",
                "Waste management company",
                "Watch company",
                "Water industry company",
                "Water services company",
                "Water supply company",
                "Water treatment company",
                "Wealth management company",
                "Weapon company",
                "Weather forecasting company",
                "Web advertising company",
                "Web analytics company",
                "Web design company",
                "Web developer company",
                "Web development company",
                "Web hosting company",
                "Web hosting service company",
                "Web marketing company",
                "Weekly newspaper company",
                "Welding company",
                "Wholesale company",
                "Wholesaling company",
                "Wi-fi company",
                "Wiki company",
                "Wind energy company",
                "Wind power company",
                "Wind turbine company",
                "Window company",
                "Window covering company",
                "Wine company",
                "Wineries company",
                "Winery company",
                "Wireless company",
                "Zoo",
                // "US State"
                // "US County"
                // "City"
            }
            .Select(t => t.ToLowerInvariant())
            .Distinct()
            .ToHashSet();

        /**********************************************************************************************************
         * CONSTRUCTORS
         **********************************************************************************************************/

        public KnowledgeGraphExternalSearchProvider()
            : base(Core.Constants.ExternalSearchProviders.GoogleKnowledgeGraphId, DefaultAcceptedEntityTypes)
        {
        }

        /**********************************************************************************************************
         * METHODS
         **********************************************************************************************************/

        public IEnumerable<EntityType> Accepts(IDictionary<string, object> config, IProvider provider) => this.Accepts(config);

        private IEnumerable<EntityType> Accepts(IDictionary<string, object> config)
            => Accepts(new KnowledgeGraphExternalSearchJobData(config));

        private IEnumerable<EntityType> Accepts(KnowledgeGraphExternalSearchJobData config)
        {
            if (!string.IsNullOrWhiteSpace(config.AcceptedEntityType))
            {
                // If configured, only accept the configured entity types
                return new EntityType[] { config.AcceptedEntityType };
            }

            // Fallback to default accepted entity types
            return DefaultAcceptedEntityTypes;
        }

        private bool Accepts(KnowledgeGraphExternalSearchJobData config, EntityType entityTypeToEvaluate)
        {
            var configurableAcceptedEntityTypes = this.Accepts(config).ToArray();

            return configurableAcceptedEntityTypes.Any(entityTypeToEvaluate.Is);
        }

        public IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
            => InternalBuildQueries(context, request, new KnowledgeGraphExternalSearchJobData(config));

        private IEnumerable<IExternalSearchQuery> InternalBuildQueries(ExecutionContext context, IExternalSearchRequest request, KnowledgeGraphExternalSearchJobData config)
        {
            if (!this.Accepts(config, request.EntityMetaData.EntityType))
                yield break;

            var existingResults = request.GetQueryResults<Result>(this).ToList();

            Func<string, bool> nameFilter = value => OrganizationFilters.NameFilter(context, value) || existingResults.Any(r => string.Equals(r.Data.name, value, StringComparison.InvariantCultureIgnoreCase));
            Func<string, bool> urlFilter = value =>  existingResults.Any(r => string.Equals(r.Data.url, value, StringComparison.InvariantCultureIgnoreCase));

            // Query Input
            var entityType          = request.EntityMetaData.EntityType;

            var configMap        = config.ToDictionary();
            var organizationName = GetValue(request, configMap, Constants.KeyName.OrganizationNameKey, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName);
            var website          = GetValue(request, configMap, Constants.KeyName.WebsiteKey, Core.Data.Vocabularies.Vocabularies.CluedInOrganization.Website);


            if (!string.IsNullOrEmpty(request.EntityMetaData.Name))
                organizationName.Add(request.EntityMetaData.Name);
            if (!string.IsNullOrEmpty(request.EntityMetaData.DisplayName))
                organizationName.Add(request.EntityMetaData.DisplayName);

            if (organizationName != null)
            {
                var values = organizationName.Select(NameNormalization.Normalize).Distinct().ToHashSet();

                foreach (var value in values.Where(v => !nameFilter(v)))
                    yield return new ExternalSearchQuery(this, entityType, ExternalSearchQueryParameter.Name, value);
            }

            website.AddRange(website.ToList().GetDomainNamesFromUris());

            if (website != null)
            {
                // This needs to be full qualified to work e.g. http://www.sitecore.net
                var values = website.Select(UriUtility.NormalizeHttpUri).Distinct();

                foreach (var value in values.Where(v => !urlFilter(v)))
                    yield return new ExternalSearchQuery(this, entityType, ExternalSearchQueryParameter.Uri, value);
            }
        }

        private static HashSet<string> GetValue(IExternalSearchRequest request, IDictionary<string, object> config, string keyName, VocabularyKey defaultKey)
        {
            HashSet<string> value;
            if (config.TryGetValue(keyName, out var customVocabKey) && !string.IsNullOrWhiteSpace(customVocabKey?.ToString()))
            {
                value = request.QueryParameters.GetValue<string, HashSet<string>>(customVocabKey.ToString(), new HashSet<string>());
            }
            else
            {
                value = request.QueryParameters.GetValue(defaultKey, new HashSet<string>());
            }

            return value;
        }

        public IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query, IDictionary<string, object> config, IProvider provider)
        {
            if (!config.TryGetValue(Constants.KeyName.ApiKey, out var key) || string.IsNullOrWhiteSpace(key?.ToString()))
                yield break;

            var name = query.QueryParameters.GetValue<string, HashSet<string>>(ExternalSearchQueryParameter.Name.ToString(), new HashSet<string>()).FirstOrDefault();
            var uri  = query.QueryParameters.GetValue<string, HashSet<string>>(ExternalSearchQueryParameter.Uri.ToString(), new HashSet<string>()).FirstOrDefault();

            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(uri))
                yield break;

            name = HttpUtility.UrlEncode(name);

            var client = new RestClient("https://kgsearch.googleapis.com");

            var queryParameters = HttpUtility.ParseQueryString("");

            if (!string.IsNullOrWhiteSpace(name))
            {
                queryParameters.Add("query", name.Trim());
            }
            if (!string.IsNullOrWhiteSpace(uri))
            {
                queryParameters.Add("query", uri.Trim());
            }
            queryParameters.Add("key", key.ToString());
            queryParameters.Add("limit", "10");
            queryParameters.Add("indent", "true");

            var request = new RestRequest($"v1/entities:search?{queryParameters}");

            var response = client.ExecuteTaskAsync<KnowledgeResponse>(request).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (response.Data.itemListElement != null)
                {
                    var elements = response.Data.itemListElement.Where(r => r.result != null && (r.result.id != null || r.result.name != null));

                    foreach (var result in elements)
                    {
                        yield return new ExternalSearchQueryResult<Result>(query, result.result);
                        yield break;
                    }
                }
            }
            else if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
                yield break;
            else if (response.ErrorException != null)
                throw new AggregateException(response.ErrorException.Message, response.ErrorException);
            else
                throw new ApplicationException("Could not execute external search query - StatusCode:" + response.StatusCode);
        }

        public IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            var resultItem = result.As<Result>();

            if (this.IsFiltered(resultItem.Data))
                yield break;

            var code = new EntityCode(request.EntityMetaData.OriginEntityCode.Type, "googleKnowledgeGraph", $"{query.QueryKey}{request.EntityMetaData.OriginEntityCode}".ToDeterministicGuid());
            var clue = new Clue(code, context.Organization) { Data = { OriginProviderDefinitionId = Id } };

            this.PopulateMetadata(clue.Data.EntityData, resultItem, request);

            yield return clue;
        }

        public IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            var resultItem = result.As<Result>();

            if (this.IsFiltered(resultItem.Data))
                return null;

            return this.CreateMetadata(resultItem, request);
        }

        public override IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            return null;
        }

        public IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            return null;
        }
        
        public ConnectionVerificationResult VerifyConnection(ExecutionContext context, IReadOnlyDictionary<string, object> config)
        {
            var hasKey = config.TryGetValue(Constants.KeyName.ApiKey, out var key);
            var name = HttpUtility.UrlEncode("Google");

            var client = new RestClient("https://kgsearch.googleapis.com");

            var queryParameters = HttpUtility.ParseQueryString("");

            if (!string.IsNullOrWhiteSpace(name))
            {
                queryParameters.Add("query", name.Trim());
            }
            if (hasKey)
            {
                queryParameters.Add("key", key.ToString());
            }
            queryParameters.Add("limit", "10");
            queryParameters.Add("indent", "true");

            var request = new RestRequest($"v1/entities:search?{queryParameters}");

            var response = client.ExecuteAsync<KnowledgeResponse>(request).Result;

            return ConstructVerifyConnectionResponse(response);
        }

        private ConnectionVerificationResult ConstructVerifyConnectionResponse(IRestResponse response)
        {
            var errorMessageBase = $"{Constants.ProviderName} returned \"{(int)response.StatusCode} {response.StatusDescription}\".";
            if (response.ErrorException != null)
            {
                return new ConnectionVerificationResult(false, $"{errorMessageBase} {(!string.IsNullOrWhiteSpace(response.ErrorException.Message) ? response.ErrorException.Message : "This could be due to breaking changes in the external system")}.");
            }

            if (response.StatusCode is HttpStatusCode.Unauthorized)
            {
                return new ConnectionVerificationResult(false, $"{errorMessageBase} This could be due to invalid API key.");
            }

            var regex = new Regex(@"\<(html|head|body|div|span|img|p\>|a href)", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace);
            var isHtml = regex.IsMatch(response.Content);

            var errorMessage = response.IsSuccessful ? string.Empty
                : string.IsNullOrWhiteSpace(response.Content) || isHtml
                    ? $"{errorMessageBase} This could be due to breaking changes in the external system."
                    : ConstructErrorResponse(errorMessageBase, response.Content);

            return new ConnectionVerificationResult(response.IsSuccessful, errorMessage);
        }

        private string ConstructErrorResponse(string errorMessageBase, string content)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<ErrorResponse>(content);

                return string.IsNullOrWhiteSpace(response?.Error?.Message) ? $"{errorMessageBase} {content}" : $"{errorMessageBase} {response.Error.Message}";
            }
            catch (Exception)
            {
                return $"{errorMessageBase} {content}";
            }
        }

        private bool IsFiltered(Result result)
        {
            if (result == null)
                return true;

            if (result.type != null && result.type.Any(t => this.typeNames.Contains(t.ToLowerInvariant())))
                return false;

            if (result.description != null &&
                    (this.typeNames.Contains(result.description.ToLowerInvariant())
                    ||
                    this.typeNames.Any(t => result.description.ToLowerInvariant().Contains(t))
                    ))
                return false;

            return true;
        }

        private IEntityMetadata CreateMetadata(IExternalSearchQueryResult<Result> resultItem, IExternalSearchRequest request)
        {
            var metadata = new EntityMetadataPart();

            this.PopulateMetadata(metadata, resultItem, request);

            return metadata;
        }

        private void PopulateMetadata(IEntityMetadata metadata, IExternalSearchQueryResult<Result> resultItem, IExternalSearchRequest request)
        {
            var code = new EntityCode(request.EntityMetaData.OriginEntityCode.Type, "googleKnowledgeGraph", $"{request.Queries.FirstOrDefault()?.QueryKey}{request.EntityMetaData.OriginEntityCode}".ToDeterministicGuid());

            metadata.EntityType         = request.EntityMetaData.EntityType;
            metadata.Name               = request.EntityMetaData.Name;
            metadata.OriginEntityCode   = code;
            metadata.Codes.Add(request.EntityMetaData.OriginEntityCode);

            metadata.Description        = resultItem.Data.detailedDescription.PrintIfAvailable(v => v.articleBody) ?? resultItem.Data.description;

            metadata.Properties[KnowledgeGraphVocabulary.Organization.Url]                          = resultItem.Data.url;
            metadata.Properties[KnowledgeGraphVocabulary.Organization.Description]                  = resultItem.Data.description;
            metadata.Properties[KnowledgeGraphVocabulary.Organization.DetailedDescriptionBody]      = resultItem.Data.detailedDescription.PrintIfAvailable(v => v.articleBody);
            metadata.Properties[KnowledgeGraphVocabulary.Organization.DetailedDescriptionLicense]   = resultItem.Data.detailedDescription.PrintIfAvailable(v => v.license);
            metadata.Properties[KnowledgeGraphVocabulary.Organization.DetailedDescriptionUrl]       = resultItem.Data.detailedDescription.PrintIfAvailable(v => v.url);

            foreach (var tag in resultItem.Data.type ?? (IEnumerable<string>)Array.Empty<string>())
            {
                metadata.Tags.Add(new Tag(tag));
            }

            Uri uri;

            if (resultItem.Data.url != null && Uri.TryCreate(resultItem.Data.url, UriKind.Absolute, out uri))
                metadata.Uri = uri;
        }

        // Since this is a configurable external search provider, theses methods should never be called
        public override bool Accepts(EntityType entityType) => throw new NotSupportedException();
        public override IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request) => throw new NotSupportedException();
        public override IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query) => throw new NotSupportedException();
        public override IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, IExternalSearchRequest request) => throw new NotSupportedException();
        public override IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request) => throw new NotSupportedException();

        /**********************************************************************************************************
         * PROPERTIES
         **********************************************************************************************************/

        public string Icon { get; } = Constants.Icon;
        public string Domain { get; } = Constants.Domain;
        public string About { get; } = Constants.About;

        public AuthMethods AuthMethods { get; } = Constants.AuthMethods;
        public IEnumerable<Control> Properties { get; } = Constants.Properties;
        public Guide Guide { get; } = Constants.Guide;
        public IntegrationType Type { get; } = Constants.IntegrationType;
    }
}

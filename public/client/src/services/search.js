import axios from "axios";

const config = {
  clientId: "react.client",
  searchUri: `${process.env.REACT_APP_API_URL}/api/ApiSearch/Search`,
  scopes: [],
};
export const search = (keyword) => {
  var token = localStorage.getItem("access_token");
  //if (token) {
  axios.defaults.headers.common["Authorization"] = token
    ? "Bearer " + token
    : "";
  return axios.get(config.searchUri + "/" + keyword);
  /* } else {
        return null;
    } */
};
export default function getLinkRoute(contentType, contentTypeStr, id) {
  switch (contentType) {
    case 100:
      return {
        pathname: "/about",
      };
    case 101:
      return {
        pathname: "/assembly",
      };
    case 102:
      return {
        pathname: "/board",
      };
    case 103:
      return {
        pathname: "/team",
      };
    case 104:
      return {
        pathname: "/former",
      };
    case 105:
      return {
        pathname: "/location",
      };
    case 106:
      return {
        pathname: "/partners",
      };
    case 107:
      return {
        pathname: "/splan",
      };
    case 108:
      return {
        pathname: "/chamberlaw",
      };
    case 109:
      return {
        pathname: "/projects",
      };
    /////

    case 120:
      return {
        pathname: "/services",
        state: {
          serviceOwnerId: 8,
          eventKey: "Academy",
          serviceTypeId: id,
        },
      };
    case 121:
      return {
        pathname: "/services",
        state: {
          serviceOwnerId: 8,
          eventKey: "Academy",
          serviceTypeId: id,
        },
      };
    case 122:
      return {
        pathname: "/services",
        state: {
          serviceOwnerId: 16,
          eventKey: "Arbitration",
          serviceTypeId: id,
        },
      };
    case 123:
      return {
        pathname: "/services",
        state: {
          serviceOwnerId: 64,
          eventKey: "TradeFacilitation",
          serviceTypeId: id,
        },
      };
    case 124:
      return {
        pathname: "/services",
        state: {
          serviceOwnerId: 4,
          eventKey: "TradePromotion",
          serviceTypeId: id,
        },
      };
    case 125:
      return {
        pathname: "/matchMaking",
        state: {
          eventKey: "MatchMaking",
        },
      };
    //case 126:
    //////
    case 130:
      return {
        pathname: "/membership",
      };
    case 135:
      return {
        pathname: "/events",
      };
    //////
    case 140:
      return {
        pathname: "/news",
      };
    case 141:
      return {
        pathname: "/news",
      };
    case 142:
      return {
        pathname: "/textMedias",
        state: {
          mediaCategory: 2,
          eventKey: "Press Release",
        },
      };
    case 143:
      return {
        pathname: "/textMedias",
        state: {
          mediaCategory: 3,
          eventKey: "Speech",
        },
      };
    case 144:
      return {
        pathname: "/textMedias",
        state: {
          mediaCategory: 1,
          eventKey: "Announcement",
        },
      };
    case 145:
      return {
        pathname: "/textMedias",
        state: {
          mediaCategory: 4,
          eventKey: "Blog",
        },
      };
    case 146:
      return {
        pathname: "/picture",
      };
    case 147:
      return {
        pathname: "/video",
      };
    /////
    case 150:
      return {
        pathname: "/eresources",
        state: {
          typeId: 0,
          typeText: "e-Resources",
        },
      };
    case 151:
      return {
        pathname: "/businessDirectory",
      };
    case 152:
      return {
        pathname: "/ppDialog",
        state: {
          categoryId: id,
        },
      };
    /////
    case 160:
      return {
        pathname: "/investmentProfile",
        state: {
          typeId: 3,
        },
      };
    case 161:
      return {
        pathname: "/investmentProfile",
        state: {
          typeId: 3,
        },
      };
    case 162:
      return {
        pathname: "/investmentProfile",
        state: {
          typeId: 1,
        },
      };
    case 163:
      return {
        pathname: "/investmentProfile",
        state: {
          typeId: 2,
        },
      };
    ///
    case 170:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 1,
          eventKey: "Trade",
          lawType: 1,
        },
      };
    case 171:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 1,
          eventKey: "Trade",
          lawType: 2,
        },
      };
    case 172:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 1,
          eventKey: "Trade",
          lawType: 3,
        },
      };
    case 173:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 2,
          eventKey: "Investment",
          lawType: 1,
        },
      };
    case 174:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 2,
          eventKey: "Investment",
          lawType: 2,
        },
      };
    case 175:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 2,
          eventKey: "Investment",
          lawType: 3,
        },
      };
    //
    case 176:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 3,
          eventKey: "Customs",
          lawType: 1,
        },
      };
    case 177:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 3,
          eventKey: "Customs",
          lawType: 2,
        },
      };
    case 178:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 3,
          eventKey: "Customs",
          lawType: 3,
        },
      };
    //
    case 181:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 4,
          eventKey: "Domestic",
          lawType: 1,
        },
      };
    case 181:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 4,
          eventKey: "Domestic",
          lawType: 2,
        },
      };
    case 182:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 4,
          eventKey: "Domestic",
          lawType: 3,
        },
      };
    //
    case 183:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 5,
          eventKey: "Land",
          lawType: 1,
        },
      };
    case 185:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 5,
          eventKey: "Land",
          lawType: 2,
        },
      };
    case 185:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 5,
          eventKey: "Land",
          lawType: 3,
        },
      };
    //
    case 186:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 99,
          eventKey: "Other",
          lawType: 1,
        },
      };
    case 187:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 99,
          eventKey: "Other",
          lawType: 2,
        },
      };
    case 188:
      return {
        pathname: "/laws",
        state: {
          lawCategory: 99,
          eventKey: "Other",
          lawType: 3,
        },
      };
    ///////
    case 191:
      return {
        pathname: "/issueForm",
      };
    case 190:
      return {
        pathname: "/contact",
      };
  }
}

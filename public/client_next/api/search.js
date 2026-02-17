import axios from "axios";

const config = {
  clientId: "react.client",
  searchUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiSearch/Search`,
  scopes: [],
};
export const search = (keyword) => {
  return axios.get(config.searchUri + "/" + keyword);
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
    case 110:
      return {
        pathname: "/formersg",
      };
    /////

    case 120:
      return {
        pathname: "/services/chamberacademy",
      };
    case 121:
      return {
        pathname: "/services/chamberacademy",
      };
    case 122:
      return {
        pathname: "/services/arbitration",
      };
    case 123:
      return {
        pathname: "/services/tradefacilitation",
      };
    case 124:
      return {
        pathname: "/services/investment&tradepromotion",
      };
    case 125:
      return {
        pathname: "/matchMaking",
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
        pathname: "/media/pressrelease",
      };
    // case 143:
    //   return {
    //     pathname: "/media",
    //     state: {
    //       mediaCategory: 3,
    //       eventKey: "Speech",
    //     },
    //   };
    case 144:
      return {
        pathname: "/media/announcement",
      };
    case 145:
      return {
        pathname: "/media/blog",
      };
    case 146:
      return {
        pathname: "/gallery/photoGallery",
      };
    case 147:
      return {
        pathname: "/gallery/videoGallery",
      };
    /////
    case 150:
      return {
        pathname: "/eresources",
      };
    case 151:
      return {
        pathname: "/businessDirectory",
      };
    case 152:
      return {
        pathname: "/ppDialog",
        query: {
          categoryId: id,
        },
      };
    /////
    case 160:
      return {
        pathname: "/investmentProfile/ethiopia",
      };
    case 161:
      return {
        pathname: "/investmentProfile/ethiopia",
      };
    case 162:
      return {
        pathname: "/investmentProfile/regions",
      };
    case 163:
      return {
        pathname: "/investmentProfile/sectors",
      };
    ///
    case 170:
      return {
        pathname: "/laws/trade/proclamation",
      };
    case 171:
      return {
        pathname: "/laws/trade/regulation",
      };
    case 172:
      return {
        pathname: "/laws/trade/directive",
      };
    case 173:
      return {
        pathname: "/laws/investment/proclamation",
      };
    case 174:
      return {
        pathname: "/laws/investment/regulation",
      };
    case 175:
      return {
        pathname: "/laws/investment/directive",
      };
    //
    case 176:
      return {
        pathname: "/laws/customs/proclamation",
      };
    case 177:
      return {
        pathname: "/laws/customs/regulation",
      };
    case 178:
      return {
        pathname: "/laws/customs/directive",
      };
    //
    case 181:
      return {
        pathname: "/laws/domestic/proclamation",
      };
    case 181:
      return {
        pathname: "/laws/domestic/regulation",
      };
    case 182:
      return {
        pathname: "/laws/domestic/directive",
      };
    //
    case 183:
      return {
        pathname: "/laws/land/proclamation",
      };
    case 185:
      return {
        pathname: "/laws/land/regulation",
      };
    case 185:
      return {
        pathname: "/laws/land/directive",
      };
    //
    case 186:
      return {
        pathname: "/laws/other/proclamation",
      };
    case 187:
      return {
        pathname: "/laws/other/regulation",
      };
    case 188:
      return {
        pathname: "/laws/other/directive",
      };
    ///////
    case 191:
      return {
        pathname: "/contactUs/issueForm",
      };
    case 190:
      return {
        pathname: "/contactUs",
      };
    default:
      return {
        pathname: "#",
      };
  }
}

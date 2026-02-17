import { lawCategories } from "./laws";
import { textMediaTypes } from "./otherMedia";
import axios from "axios";

const config = {
  clientId: "react.client",
  mainMenusUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiWebsite/Menus`,
  translationsUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiWebsite/Translations`,
  footerMenusUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiWebsite/FooterMenu`,
  mediaTypeUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiWebsite/MediaTypeByUrl`,
  getContentUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiWebsite/GetContent`,
  quickLinkContentsUri: `${process.env.NEXT_PUBLIC_API_URL}/api/ApiWebsite/QuickLinkContents`,
  scopes: [],
};

export const getMainMenus = () => {
  return axios.get(config.mainMenusUri);
};
export const getTranslations = () => {
  return axios.get(config.translationsUri);
};
export const getFooterMenu = () => {
  return axios.get(config.footerMenusUri);
};
export const getMediaType = (menuId) => {
  return axios.get(config.mediaTypeUri + "/" + menuId);
};
export const getContent = (id) => {
  return axios.get(config.getContentUri + "/" + id);
};
export const getQuickLinkContents = (id) => {
  return axios.get(config.quickLinkContentsUri + "/" + id);
};
export const getLocalByKey = (key, translations, locale) => {
  if (
    !translations ||
    !translations.find ||
    key == null ||
    key == undefined ||
    key.trim().length <= 0
  )
    return key;
  const newKey = key.replaceAll(" ", "").toLowerCase();
  const localData = translations.find(
    (t) => t.key.replaceAll(" ", "").toLowerCase() == newKey
  );
  if (!localData) return key;
  const localVal = localData.local ? localData.local.trim() : "";
  return locale == "en" || !localVal ? localData.default : localData.local;
};
export const getLocalForVal = (eng, local, lang) => {
  return lang == "en" || !local ? eng : local;
};

export const getPathFromTitle = (title) => {
  if (title) {
    return title.toLowerCase().replaceAll(" ", "").replaceAll("&", "_");
  }
  return "";
};

export const userMenu = [
  {
    text: "Requests",
    title: "Requests By You",
    path: "/myServices?navTo=request",
    businessAccountMenu: false,
  },
  {
    text: "Surveys",
    title: "Surveys",
    path: "/myServices?navTo=survey",
    businessAccountMenu: true,
  },
  {
    text: "Legal Documents",
    title: "Legal Documents",
    path: "/myServices?navTo=legalProfile",
    businessAccountMenu: true,
  },
  {
    text: "Export Agreements",
    title: "Export Agreements",
    path: "/myServices?navTo=exportAgreement",
    businessAccountMenu: true,
  },
  {
    text: "Contacts",
    title: "Contacts",
    path: "/myServices?navTo=contacts",
    businessAccountMenu: true,
  },
  {
    text: "Membership",
    title: "Membership",
    path: "/myServices?navTo=membership",
    businessAccountMenu: true,
  },
];

export const userOtherMenus = [
  {
    text: "Update Profile",
    title: "Update Profile",
    path: "/profileForm",
  },
  {
    text: "Change Password",
    title: "Change Password",
    path: "/changePassword",
  },
];

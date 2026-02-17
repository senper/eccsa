import { Grid } from "@mui/material";
import { useRouter } from "next/router";
import React from "react";
import { CommonBreadcrumb } from "../Components/Layout/CommonBreadcrumb";
import MultiContentPage from "../Components/OtherPages/MultiContentPage";
import OtherPageLinks from "../Components/OtherPages/OtherPageLinks";
import SingleContentPage from "../Components/OtherPages/SingleContentPage";
import Custom404 from "./404";
import {
  getContent,
  getLocalByKey,
  getLocalForVal,
  getMediaType,
  getQuickLinkContents,
  getTranslations,
} from "./api/menu";

export default function OtherPage({
  propContents,
  quickLinkHeader,
  mediaType,
  quickLinks,
  translations,
}) {
  const location = useRouter();
  const { locale } = location;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };

  const { type } = location.query;

  const [contents, setContents] = React.useState(propContents);
  const [loading, setLoading] = React.useState(false);
  const [loadingContent, setLoadingContent] = React.useState(false);
  const [loadingContentsList, setLoadingContentsList] = React.useState(false);
  
  const [subQuickLinkId, setSubQuickLinkId] = React.useState(
    mediaType ? mediaType.quickLinkId : null
  );
  const [selectedContent, setSelectedContent] = React.useState(null);

  if (!mediaType) return <Custom404 statusCode={404} />;

  function loadQLContents(id) {
    setSubQuickLinkId(id);
    setLoadingContentsList(true);
    getQuickLinkContents(id)
      .then((data) => {
        if(data.data.isPath && data.data.path) {
          location.push(data.data.path);
        } else {
          setContents(data.data.$values);
          setLoadingContentsList(false);
        }
      })
      .catch((error) => {
        // console.log(error.response);
        setLoadingContentsList(false);
      });
  }

  function loadSelectedContent(id) {
    setLoadingContent(true);
    getContent(id)
      .then((data) => {
        setSelectedContent(data.data);
        setLoadingContent(false);
      })
      .catch((error) => {
        // console.log(error.response);
        setLoadingContent(false);
      });
  }

  const hasQuickLinks = quickLinkHeader && quickLinks.length > 0;

  const SelectPageType = () => {
    if (!contents) return null;
    if (contents.length == 1) {
      return (
        <SingleContentPage
          getLocal={getLocal}
          getLocalByVal={getLocalByVal}
          contents={contents}
          loadingContentsList={loadingContentsList}
        />
      );
    } else if (contents.length > 1) {
      return (
        <MultiContentPage
          getLocal={getLocal}
          getLocalByVal={getLocalByVal}
          contents={contents}
          selectedContent={selectedContent}
          setSelectedContent={setSelectedContent}
          loadSelectedContent={loadSelectedContent}
          loadingContent={loadingContent}
          loadingContentsList={loadingContentsList}
        />
      );
    }
  };

  const breadcrumbLinks = () => {
    const mainBML = [
      {
        title: getLocal("Home"),
        path: "/",
      },
      {
        title: mediaType && mediaType.menu ? mediaType.menu.title : "#",
        path: mediaType ? `${type[0]}` : "#",
      },
    ];

    if (
      subQuickLinkId &&
      quickLinks &&
      quickLinks.find((q) => q.id == subQuickLinkId)
    ) {
      var subQL = quickLinks.find((q) => q.id == subQuickLinkId);
      if (!mediaType.menu || subQL.title.toLowerCase() != mediaType.menu.title.toLowerCase()) {
        mainBML.push({
          title: subQL.title,
          path: "#",
        });
      }
    }

    return mainBML;
  };
  return (
    <>
      <div className="paddedMainDiv">
        <CommonBreadcrumb links={breadcrumbLinks()} />
        <Grid container>
          <Grid item xs={11} md={hasQuickLinks ? 9 : 11}>
            {SelectPageType()}
            {!contents || contents.length <= 0 ? (
              <strong>{getLocal("Empty Content")}</strong>
            ) : null}
          </Grid>
          {hasQuickLinks ? (
            <Grid item xs={12} md={3}>
              <OtherPageLinks
                loading={loading}
                title={getLocalByKey(
                  quickLinkHeader.label,
                  quickLinkHeader.labelLocal
                )}
                links={quickLinks}
                getLocal={getLocal}
                getLocalByVal={getLocalByVal}
                loadQLContents={loadQLContents}
              />
            </Grid>
          ) : null}
        </Grid>
      </div>
    </>
  );
}

export async function getServerSideProps(context) {
  const { locale, res } = context;
  res.setHeader(
    "Cache-Control",
    "public, s-maxage=10, stale-while-revalidate=59"
  );
  const { type } = context.query;
  let translations = [];
  await getTranslations()
    .then((data) => {
      translations = data.data.$values;
    })
    .catch((error) => {});

  let propContents = [];
  let quickLinkHeader = null;
  let mediaType = null;
  let quickLinks = [];
  if (type.length == 1) {
    await getMediaType(type[0])
      .then((data) => {
        if (data.data) {
          mediaType = {
            name: getLocalForVal(data.data.name, data.data.nameLocal, locale),
            menuId: data.data.menuId,
            quickLinkId: data.data && data.data.quickLinkId ? data.data.quickLinkId : null,
            menu: {
              title: data.data.menu
                ? getLocalForVal(
                    data.data.menu.title,
                    data.data.menu.titleLocal,
                    locale
                  )
                : "",
            },
          };
          if (data.data.quickLinkHeader) {
            quickLinkHeader = {
              label: getLocalForVal(
                data.data.quickLinkHeader.label,
                data.data.quickLinkHeader.labelLocal,
                locale
              ),
            };
            data.data.quickLinkHeader.quickLinks.$values.forEach((s) => {
              quickLinks.push({
                ...s,
                title: getLocalForVal(s.title, s.titleLocal, locale),
              });
            });
          }
          data.data.contents.$values.forEach((s) => {
            propContents.push({
              ...s,
              title: getLocalForVal(s.title, s.titleLocal, locale),
              content: getLocalForVal(s.content, s.contentLocal, locale),
            });
          });
        }
      })
      .catch((error) => {});
  }
  return {
    props: {
      propContents,
      quickLinkHeader,
      mediaType,
      quickLinks,
      translations,
    },
  };
}

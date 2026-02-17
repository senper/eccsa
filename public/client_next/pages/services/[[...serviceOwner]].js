import { useRouter } from "next/router";
import React from "react";
import { getServiceOwners, getServiceTypes } from "../api/servicetype";
import { CommonBreadcrumb } from "../../Components/Layout/CommonBreadcrumb";
import { Grid } from "@mui/material";
import { OwnerBox } from "../../Components/Services/OwnerBox";
import {
  getLocalByKey,
  getLocalForVal,
  getPathFromTitle,
  getTranslations,
} from "../api/menu";
import Custom404 from "../404";

export default function Services({
  serviceOwners,
  serviceOwnerId,
  serviceTypes,
  translations,
}) {
  const router = useRouter();
  const { locale } = router;
  const { selectedServiceTypeId, serviceOwner } = router.query;
  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };

  const breadcrumbLinks = [
    {
      title: getLocal("Home"),
      path: "/",
    },
    {
      title: getLocal("Services"),
      path: "/services",
    },
  ];

  if (serviceOwner && !serviceOwnerId) {
    return <Custom404 />;
  }
  return (
    <>
      <div className="paddedMainDiv">
        <CommonBreadcrumb links={breadcrumbLinks} />
        <Grid container spacing={1}>
          {serviceOwners
            ? serviceOwners.map((so, index) => (
                <OwnerBox
                  so={so}
                  serviceOwnerId={serviceOwnerId}
                  serviceTypes={serviceTypes}
                  selectedServiceTypeId={selectedServiceTypeId}
                  lang={locale}
                  key={index}
                  getLocal={getLocal}
                />
              ))
            : null}
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
  let serviceOwnerId = 0;
  const { serviceOwner } = context.query;
  function compare(a, b) {
    if (serviceOwner && serviceOwner[0] === getPathFromTitle(a.title)) {
      return -1;
    }
    if (serviceOwner && serviceOwner[0] === getPathFromTitle(b.title)) {
      return 1;
    }
    return 0;
  }

  let translations = [];
  await getTranslations()
    .then((data) => {
      translations = data.data.$values;
    })
    .catch((error) => {});
  let serviceOwners = [];
  await getServiceOwners()
    .then((data) => {
      data.data.$values.sort(compare).forEach((so) => {
        serviceOwners.push({
          ...so,
          title: getLocalForVal(so.title, so.titleLocal, locale),
          description: getLocalForVal(
            so.description,
            so.descriptionLocal,
            locale
          ),
        });
      });
    })
    .catch((error) => {});

  let serviceTypes = [];
  await getServiceTypes()
    .then((data) => {
      data.data.$values.forEach((s) => {
        serviceTypes.push({
          ...s,
          name: getLocalForVal(s.nameEng, s.nameLocal, locale),
          description: getLocalForVal(
            s.descriptionEng ?? "",
            s.descriptionLocal ?? "",
            locale
          ),
        });
      });
    })
    .catch((error) => {});

  if (serviceOwner && serviceOwner.length == 1) {
    const selectedOwner = serviceOwners.find(
      (t) => getPathFromTitle(t.title) == serviceOwner[0].toLowerCase()
    );
    if (selectedOwner) {
      serviceOwnerId = selectedOwner.idInt;
    }
  }

  return {
    props: {
      serviceOwners,
      serviceOwnerId,
      serviceTypes,
      translations,
    },
  };
}

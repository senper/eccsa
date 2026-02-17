import React from "react";
import { getLawsList, lawCategories, lawTypes } from "../api/laws";
import { useRouter } from "next/router";
import { AddAlert } from "@mui/icons-material";
import { Grid, Snackbar } from "@mui/material";
import { CommonBreadcrumb } from "../../Components/Layout/CommonBreadcrumb";
import { getLocalByKey, getPathFromTitle } from "../api/menu";
import LawForm from "../../Components/Law/LawForm";
import LawsList from "../../Components/Law/LawsList";
import Custom404 from "../404";

export default function Laws({ translations, lawsList, lawCategoryValue, lawTypeValue }) {
  const location = useRouter(); ///
  const { locale } = location;

  const { category } = location.query;
  //
  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(false);
  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
            setErrorMsg("");
          }, 6000);
        }
        break;
      default:
        break;
    }
  };
  //
  const [loading, setLoading] = React.useState(false);
  const [laws, setLaws] = React.useState([]);
  const [selectedLawType, setSelectedLawType] = React.useState(lawTypeValue ? lawTypeValue : 1);
 
  React.useEffect(() => {
    //loadLaws(selectedLawType);
    setLaws(lawsList);
    setSelectedLawType(lawTypeValue);
  }, [lawsList, lawTypeValue]);

  function loadLaws(type) {
    if (!lawCategoryValue) return;
    const lawType = lawTypes.find(
      (t) => t.value == type
    );
    location.push("/laws/" + category[0] + "/"+ getPathFromTitle(lawType.text))
    /* setLaws([]);
    setLoading(true);
    getLawsList(lawCategoryValue, type)
      .then((data) => {
        setLaws(data.data.$values);
        setLoading(false);
      })
      .catch((error) => {
        setLoading(false);
      }); */
  }
  const breadcrumbLinks = () => {
    var categoryVal = lawCategoryValue
      ? lawCategoryValue
      : lawCategories[0].value;
    const category = lawCategories.find((s) => s.value == categoryVal);
    return [
      {
        title: getLocal("Home"),
        path: "/",
      },
      {
        title:
          (categoryVal && category
            ? getLocalByVal(category.text, category.textLocal) + " "
            : "") + getLocal("Laws"),
        path: "#",
      },
    ];
  };

  if (
    !category ||
    (category.length !== 1 && category.length !== 2) ||
    !lawCategoryValue ||
    (category.length == 2 && !lawTypeValue)
  ) {
    return <Custom404 />;
  }
  return (
    <>
      <div className="paddedMainDiv">
        <Grid container spacing={2}>
          <Grid item xs={2} md={2}>
            <CommonBreadcrumb links={breadcrumbLinks()} />
          </Grid>
          <Grid item xs={9} md={9}>
            <LawForm
              selectedLawType={selectedLawType}
              setSelectedLawType={setSelectedLawType}
              getLocalByVal={getLocalByVal}
              loadLaws={loadLaws}
            />
          </Grid>
          <Grid item xs={12} md={12}>
            {loading ? <h3>{getLocal("Loading")}...</h3> : null}
            {laws && laws.length > 0 ? (
              <LawsList
                laws={laws}
                getLocal={getLocal}
                getLocalByVal={getLocalByVal}
                showNotification={showNotification}
                locale={locale}
                setErrorMsg={setErrorMsg}
              />
            ) : null}
            {!loading && (!laws || laws.length <= 0) ? (
              <h3>{getLocal("NOTHING FOUND")}</h3>
            ) : null}
          </Grid>
        </Grid>
      </div>

      <Snackbar
        place="tr"
        color="danger"
        icon={AddAlert}
        message={errorMsg ? errorMsg : getLocal("Failed to Download")}
        open={error}
        onClose={() => setError(false)}
      />
    </>
  );
}

export async function getServerSideProps(context) {  
  const { category } = context.query;
  let lawCategoryValue = 0;
  let lawTypeValue = 1;
  //
  if (category && (category.length == 1 || category.length == 2)) {
    const selectedCategory = lawCategories.find(
      (t) => getPathFromTitle(t.text) == category[0].toLowerCase()
    );
    if (selectedCategory) {
      lawCategoryValue = selectedCategory.value;
      if (category.length == 2) {
        const selectedType = lawTypes.find(
          (t) => getPathFromTitle(t.text) == category[1].toLowerCase()
        );
        if (selectedType) {
          lawTypeValue = selectedType.value;
        }
      }
    }
  }
  //
  let lawsList = [];
  await getLawsList(lawCategoryValue, lawTypeValue)
      .then((data) => {
        lawsList = data.data.$values;
      });
  
  return {
    props: {
      lawsList,
      lawCategoryValue,
      lawTypeValue
    },
  };
}

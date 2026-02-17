import React from "react";

// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
// core components
import GridContainer from "components/vender/Grid/GridContainer.js";
import GridItem from "components/vender/Grid/GridItem.js";
import Button from "components/vender/CustomButtons/Button.js";
import Card from "components/vender/Card/Card.js";
import CardBody from "components/vender/Card/CardBody.js";
import CardIcon from "components/vender/Card/CardIcon.js";
import CardHeader from "components/vender/Card/CardHeader.js";

import { responsibleUnits, messageTemplates } from "variables/lookups.js";

import { cardTitle } from "assets/jss/material-dashboard-pro-react.js";
import { Link, useLocation } from "react-router-dom";

import FormControl from "@material-ui/core/FormControl";
import InputLabel from "@material-ui/core/InputLabel";
import Select from "@material-ui/core/Select";
import MenuItem from "@material-ui/core/MenuItem";
import { Edit, Info } from "@material-ui/icons";
import btnstyles from "assets/jss/material-dashboard-pro-react/views/buttonsStyle.js";
import { renderCellExpand } from "components/common/MaterialUIDataGrid/renderCellExpand";
import CustomDataGrid from "components/common/MaterialUIDataGrid/CustomDataGrid";

const styles = {
  cardIconTitle: {
    ...cardTitle,
    marginTop: "15px",
    marginBottom: "0px",
  },
};

const useStyles = makeStyles(styles);

const useBtnStyles = makeStyles(btnstyles);

const dataRows = messageTemplates;

export default function MessageTemplates() {
  const location = useLocation();
  const classes = useStyles();
  const btnClasses = useBtnStyles();

  const [rows, setRows] = React.useState(dataRows);

  React.useEffect(() => {
    setRows(dataRows);
  }, [dataRows]);

  const columns = [
    {
      headerName: "Message",
      field: "message",
      renderCell: renderCellExpand,
      flex: 0.3,
    },
    {
      headerName: "Media Type",
      field: "media_type",
      flex: 0.08,
    },
    {
      headerName: "Topic",
      field: "topic",
      flex: 0.08,
    },
    {
      headerName: "Message Type",
      field: "message_type",
      flex: 0.1,
    },
    {
      headerName: "Is Recurrent",
      field: "is_recurrent",
      flex: 0.08,
    },
    {
      headerName: "Actions",
      field: "actions",
      flex: 0.08,
      filterable: false,
      sortable: false,
      // eslint-disable-next-line react/display-name
      renderCell: () => (
        <div className="actions-right">
          {/* use this button to add a like kind of action */}

          <Button
            justIcon
            round
            color="primary"
            size="sm"
            className={btnClasses.marginRight}
          >
            <Link
              style={{ color: "white" }}
              to={{
                pathname: location.state.layout + "/message_template_edit",
                state: { unit: responsibleUnit },
              }}
            >
              <Edit className={btnClasses.icons} />
            </Link>
          </Button>
          <Button
            justIcon
            round
            color="info"
            size="sm"
            className={btnClasses.marginRight}
          >
            <Link
              style={{ color: "white" }}
              to={{
                pathname: location.state.layout + "/message_template_detail",
                state: { unit: responsibleUnit },
              }}
            >
              <Info className={btnClasses.icons} />
            </Link>
          </Button>
        </div>
      ),
    },
  ];

  const [responsibleUnit, setResponsibleUnit] = React.useState("");
  React.useEffect(() => {
    if (location.state) {
      setResponsibleUnit(location.state.unit);
    }
  }, [location]);

  return (
    <GridContainer>
      <GridItem xs={12}>
        <Card>
          <CardHeader color="primary" icon>
            <CardIcon color="primary">{"Message Templates"}</CardIcon>
          </CardHeader>
          <CardBody>
            <GridContainer>
              <GridItem xs={12} sm={12} md={4}>
                <FormControl fullWidth className={classes.selectFormControl}>
                  <InputLabel className={classes.selectLabel}>
                    Responsible Unit
                  </InputLabel>
                  <Select
                    MenuProps={{
                      className: classes.selectMenu,
                    }}
                    classes={{
                      select: classes.select,
                    }}
                    value={responsibleUnit}
                    inputProps={{
                      name: "ResponsibleUnit",
                      id: "responsible-unit",
                      disabled: true,
                    }}
                  >
                    <MenuItem
                      disabled
                      classes={{
                        root: classes.selectMenuItem,
                      }}
                    >
                      Choose Responsible Unit
                    </MenuItem>
                    {responsibleUnits.map((_item, index) => {
                      return (
                        <MenuItem
                          key={_item.id}
                          classes={{
                            root: classes.selectMenuItem,
                            selected: classes.selectMenuItemSelected,
                          }}
                          value={_item.id}
                        >
                          {_item.name}
                        </MenuItem>
                      );
                    })}
                  </Select>
                </FormControl>
              </GridItem>
            </GridContainer>
            <br />
            <CustomDataGrid dataRows={rows} columns={columns} />
          </CardBody>
        </Card>
      </GridItem>
    </GridContainer>
  );
}

﻿#region netDxf, Copyright(C) 2015 Daniel Carvajal, Licensed under LGPL.
// 
//                         netDxf library
//  Copyright (C) 2009-2015 Daniel Carvajal (haplokuon@gmail.com)
//  
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of the GNU Lesser General Public
//  License as published by the Free Software Foundation; either
//  version 2.1 of the License, or (at your option) any later version.
//  
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
//  
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//  FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//  COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//  IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//  CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using netDxf.Objects;
using netDxf.Tables;

namespace netDxf.Collections
{
    /// <summary>
    /// Represents a collection of dgn underlay definitions.
    /// </summary>
    public sealed class UnderlayDgnDefinitions :
        TableObjects<UnderlayDgnDefinition>
    {
        #region constructor

        internal UnderlayDgnDefinitions(DxfDocument document, string handle = null)
            : this(document, 0, handle)
        {
        }

        internal UnderlayDgnDefinitions(DxfDocument document, int capacity, string handle = null)
            : base(document,
            new Dictionary<string, UnderlayDgnDefinition>(capacity, StringComparer.OrdinalIgnoreCase),
            new Dictionary<string, List<DxfObject>>(capacity, StringComparer.OrdinalIgnoreCase),
            DxfObjectCode.UnderlayDgnDefinitionDictionary,
            handle)
        {
            this.maxCapacity = int.MaxValue;
        }

        #endregion

        #region override methods

        /// <summary>
        /// Adds a dgn underlay definition to the list.
        /// </summary>
        /// <param name="underlayDgnDefinition"><see cref="UnderlayDgnDefinition">UnderlayDgnDefinition</see> to add to the list.</param>
        /// <param name="assignHandle">Specifies if a handle needs to be generated for the underlay definition parameter.</param>
        /// <returns>
        /// If an underlay definition already exists with the same name as the instance that is being added the method returns the existing underlay definition,
        /// if not it will return the new underlay definition.
        /// </returns>
        internal override UnderlayDgnDefinition Add(UnderlayDgnDefinition underlayDgnDefinition, bool assignHandle)
        {
            if (this.list.Count >= this.maxCapacity)
                throw new OverflowException(string.Format("Table overflow. The maximum number of elements the table {0} can have is {1}", this.codeName, this.maxCapacity));

            UnderlayDgnDefinition add;
            if (this.list.TryGetValue(underlayDgnDefinition.Name, out add))
                return add;

            if (assignHandle || string.IsNullOrEmpty(underlayDgnDefinition.Handle))
                this.document.NumHandles = underlayDgnDefinition.AsignHandle(this.document.NumHandles);

            this.document.AddedObjects.Add(underlayDgnDefinition.Handle, underlayDgnDefinition);
            this.list.Add(underlayDgnDefinition.Name, underlayDgnDefinition);
            this.references.Add(underlayDgnDefinition.Name, new List<DxfObject>());

            underlayDgnDefinition.Owner = this;

            underlayDgnDefinition.NameChanged += this.Item_NameChanged;

            return underlayDgnDefinition;
        }

        /// <summary>
        /// Removes an dgn underlay definition.
        /// </summary>
        /// <param name="name"><see cref="UnderlayDgnDefinition">UnderlayDgnDefinition</see> name to remove from the document.</param>
        /// <returns>True if the underlay definition has been successfully removed, or false otherwise.</returns>
        /// <remarks>Any underlay definition referenced by objects cannot be removed.</remarks>
        public override bool Remove(string name)
        {
            return this.Remove(this[name]);
        }

        /// <summary>
        /// Removes a dgn underlay definition.
        /// </summary>
        /// <param name="underlayDgnDefinition"><see cref="UnderlayDgnDefinition">UnderlayDgnDefinition</see> to remove from the document.</param>
        /// <returns>True if the underlay definition has been successfully removed, or false otherwise.</returns>
        /// <remarks>Any underlay definition referenced by objects cannot be removed.</remarks>
        public override bool Remove(UnderlayDgnDefinition underlayDgnDefinition)
        {
            if (underlayDgnDefinition == null)
                return false;

            if (!this.Contains(underlayDgnDefinition))
                return false;

            if (underlayDgnDefinition.IsReserved)
                return false;

            if (this.references[underlayDgnDefinition.Name].Count != 0)
                return false;

            this.document.AddedObjects.Remove(underlayDgnDefinition.Handle);
            this.references.Remove(underlayDgnDefinition.Name);
            this.list.Remove(underlayDgnDefinition.Name);

            underlayDgnDefinition.Handle = null;
            underlayDgnDefinition.Owner = null;

            underlayDgnDefinition.NameChanged -= this.Item_NameChanged;

            return true;
        }

        #endregion

        #region TableObject events

        private void Item_NameChanged(TableObject sender, TableObjectChangedEventArgs<string> e)
        {
            if (this.Contains(e.NewValue))
                throw new ArgumentException("There is already another dgn underlay definition with the same name.");

            this.list.Remove(sender.Name);
            this.list.Add(e.NewValue, (UnderlayDgnDefinition)sender);

            List<DxfObject> refs = this.references[sender.Name];
            this.references.Remove(sender.Name);
            this.references.Add(e.NewValue, refs);
        }

        #endregion
    }
}
